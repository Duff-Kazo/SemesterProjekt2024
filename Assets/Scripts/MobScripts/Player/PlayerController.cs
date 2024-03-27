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
using Image = UnityEngine.UI.Image;

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
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private GameObject acidUI;
    [SerializeField] private GameObject aim;
    [SerializeField] private float reloadTime = 2;
    public bool isReloading = false;
    private PlayerWeaponAim playerWeapon;
    private PlayerAcidBall playerAcidBall;
    public int bulletCount = 0;
    [SerializeField] private int maxMagazines = 4;
    private int maxBullets = 16;
    private int magazinesCount;
    private int magazineBullets;

    //MonsterItems
    public static bool eyesActivated;
    public static bool shieldActivated = false;
    public static bool orbsActivated = false;
    public static bool plagueActivated = false;


    //Animations
    [Header("Animations")]
    [SerializeField] public int monsterState = 0;
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

    //Sounds
    [Header("Sounds")]
    [SerializeField] private AudioSource reload;
    [SerializeField] private AudioSource loaded;
    [SerializeField] private AudioSource damageSound;
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
        playerAcidBall = FindObjectOfType<PlayerAcidBall>();
        playerAcidBall.enabled = false;
        magazinesCount = maxMagazines;
        magazineBullets = magazinesCount * maxBullets;
        acidUI.SetActive(false);
        bulletUI.SetActive(true);
        aim.SetActive(true);
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

        //HandleWeaponSelect
        if(monsterState == 3)
        {
            playerWeapon.enabled = false;
            playerAcidBall.enabled = true;
            bulletUI.SetActive(false);
            acidUI.SetActive(true);
            aim.SetActive(false);
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
        if(bulletCount >= 10)
        {
            bulletText.text = ("x" + bulletCount);
        }
        else
        {
            bulletText.text = ("x0" + bulletCount);
        }
        if(magazineBullets >= 10)
        {
            magazinesText.text = ("" + magazineBullets);
        }
        else
        {
            magazinesText.text = ("0" + magazineBullets);
        }
        if(bulletCount <= 0 && !isReloading)
        {
            StartCoroutine(ReloadWeapon());
        }
    }

    public void TakeDamage(float damage)
    {
        damageSound.Play();
        StartCoroutine(DamageAnimation());
        Instantiate(blood, transform.position, Quaternion.identity);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ReloadWeapon()
    {
        if(magazineBullets > 0 && bulletCount != maxBullets)
        {
            magazinesCount -= 1;
            isReloading = true;
            playerWeapon.canShoot = false;
            reloadAnimation.value = reloadAnimation.minValue;
            reload.Play();
            yield return new WaitForSeconds(reloadTime);
            loaded.Play();
            reloadAnimation.value = reloadAnimation.maxValue;
            if (magazineBullets > maxBullets || magazineBullets < maxBullets || maxBullets == magazineBullets)
            {
                int test = magazineBullets - (maxBullets - bulletCount);
                if (test < 0)
                {
                    bulletCount += magazineBullets;
                    magazineBullets = 0;
                }
                else
                {
                    magazineBullets -= maxBullets - bulletCount;
                    bulletCount += maxBullets - bulletCount;
                }
            }
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
        SceneManager.LoadScene("DeathScreen");
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
            magazineBullets = magazinesCount * maxBullets;
            if (magazinesCount > 0 && bulletCount > 0)
            {
                bulletCount += 4;
            }
        }
        else if(itemName == "FullAuto")
        {
            ShopButtons.fullAutoBought = true;
        }
        else if (itemName == "MoreMags")
        {
            magazinesCount += 1;
            maxMagazines += 1;
            magazineBullets += maxBullets;
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
}
