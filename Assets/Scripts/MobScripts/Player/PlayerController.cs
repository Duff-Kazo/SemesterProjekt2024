using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using Cinemachine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xInput;
    private float yInput;
    private float scaleX;


    //Components
    SpriteRenderer spriteRenderer;
    private CinemachineVirtualCamera virtualCamera;

    //Movement
    [Header("Movement")]
    private float baseSpeed = 6;
    [SerializeField] private float speed;

    //Combat
    [Header("CombatSystem")]
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI magazinesText;
    [SerializeField] private Slider reloadAnimation;
    [SerializeField] private float reloadTime = 2;
    public bool isReloading = false;
    private PlayerWeaponAim playerWeapon;
    public int bulletCount = 0;
    public int magazinesCount = 4;
    private int maxMagazines = 4;
    private int maxBullets = 16;


    //Animations
    [Header("Animations")]
    [SerializeField] private int monsterState = 0;
    private string currentState = "";
    private Animator playerAnimator;

    //NO INFECTION
    const string idleAnimation = "PlayerIdleAnimation";
    const string walkAnimation = "PlayerRunAnimation";

    //INFECTIONSTATE 1
    const string idle1 = "IdleAnimationInfection1";
    const string run1 = "RunAnimationInfection1";

    //INFECTIONSTATE2
    const string idle2 = "IdleAnimationInfection2";
    const string run2 = "RunAnimationInfection2";

    //INFECTIONSTATE3
    const string idle3 = "IdleAnimationInfection3";
    const string run3 = "RunAnimationInfection3";

    //Effects
    [Header("Effects")]
    [SerializeField] private GameObject blood;

    //Health System
    [Header("Health System")]
    private float health;
    [SerializeField] private float lerpSpeed = 0.05f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthBarSlider;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    private float maxHealth = 10;

    //Shop System
    [Header("ShopSystem")]
    [SerializeField] private TextMeshProUGUI bloodPointsText;
    public int bloodPoints = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        scaleX = transform.localScale.x;
        health = maxHealth;
        bulletCount = maxBullets;
        playerWeapon = FindObjectOfType<PlayerWeaponAim>();
        magazinesCount = maxMagazines;
    }
    void Update()
    {
        if (Interactable.inShop)
        {
            return;
        }
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            bulletCount = 0;
            StartCoroutine(ReloadWeapon());
        }
    }

    private void FixedUpdate()
    {
        bloodPointsText.text = ("" + bloodPoints);
        if (Interactable.inShop)
        {
            return;
        }
        rb.velocity = new Vector2 (xInput * speed, yInput * speed);
        if(xInput == 0 && yInput == 0)
        {
            if(monsterState == 0)
            {
                ChangeAnimationState(idleAnimation);
                rb.velocity = Vector2.zero;
            }
            else if(monsterState == 1)
            {
                ChangeAnimationState(idle1);
                rb.velocity = Vector2.zero;
            }
            else if (monsterState == 2)
            {
                ChangeAnimationState(idle2);
                rb.velocity = Vector2.zero;
            }
            else if (monsterState == 3)
            {
                ChangeAnimationState(idle3);
                rb.velocity = Vector2.zero;
            }


        }
        else
        {
            if (monsterState == 0)
            {
                ChangeAnimationState(walkAnimation);
            }
            else if (monsterState == 1)
            {
                ChangeAnimationState(run1);
            }
            else if (monsterState == 2)
            {
                ChangeAnimationState(run2);
            }
            else if (monsterState == 3)
            {
                ChangeAnimationState(run3);
            }
        }

        if (xInput > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (xInput < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }


        //HEALTHBAR
        maxHealthText.text = ("" + maxHealth);
        currentHealthText.text = (health + "/");
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if(healthSlider.value != easeHealthBarSlider.value)
        {
            easeHealthBarSlider.value = Mathf.Lerp(easeHealthBarSlider.value, health, lerpSpeed);
        }


        //Bullets
        bulletText.text = ("x" + bulletCount);
        magazinesText.text = ("" + magazinesCount);
        if(bulletCount <= 0 && !isReloading)
        {
            StartCoroutine(ReloadWeapon());
        }
    }

    public void TakeDamage(float damage)
    {
        if(health > 1)
        {
            StartCoroutine(DamageAnimation());
            Instantiate(blood, transform.position, Quaternion.identity);
            health--;
        }
        else if(health <= 1)
        {
            Die();
        }
    }

    private IEnumerator ReloadWeapon()
    {
        if(magazinesCount > 0)
        {
            magazinesCount -= 1;
            isReloading = true;
            playerWeapon.canShoot = false;
            bulletCount = 0;
            reloadAnimation.value = reloadAnimation.minValue;
            yield return new WaitForSeconds(reloadTime);
            reloadAnimation.value = reloadAnimation.maxValue;
            bulletCount = maxBullets;
            playerWeapon.canShoot = true;
            isReloading = false;
        }
    }

    private IEnumerator DamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        SceneManager.LoadScene("Dungeons");
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        playerAnimator.Play(newState);
        currentState = newState;
    }


    public void GetBlood(int amount)
    {
        bloodPoints += amount;
    }


    public void BuyItem(string itemName, int itemCost)
    {
        if(itemName == "MagUpgrade")
        {
            maxBullets += 4;
            bulletCount = maxBullets;
        }
        else if(itemName == "FullAuto")
        {
            ShopButtons.fullAutoBought = true;
        }
        else if (itemName == "MoreMags")
        {
            magazinesCount += 1;
            maxMagazines += 1;
        }
        else if (itemName == "LegMods")
        {
            speed += baseSpeed * 0.2f;
        }
        else if (itemName == "Health")
        {
            healthSlider.maxValue += 1;
            easeHealthBarSlider.maxValue = healthSlider.maxValue;
            maxHealth += 1;
            health = maxHealth;
        }
        else if(itemName == "UpDamage")
        {
            ShopButtons.bulletDamage += 1 * 0.5f;
        }
    }


    public void ActivateMonsterItems(string itemName)
    {
        if(itemName == "Shield")
        {
            Debug.Log("Activate Shield");
        }
        else if (itemName == "Orbs")
        {
            Debug.Log("Activate Orbs");
        }
        if (itemName == "Plague")
        {
            Debug.Log("Activate Plague");
        }
    }
}
