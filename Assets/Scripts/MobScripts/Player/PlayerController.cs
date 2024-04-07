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

    //Dash
    [Header("Dash")]
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashLength;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private GameObject dashUI;
    [SerializeField] private GameObject dashIcon;
    private bool canDash = true;
    private Vector2 moveDirection;
    public bool dashEnabled;
    

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
    public float health;
    [SerializeField] private float lerpSpeed = 0.05f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthBarSlider;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    public float maxHealth = 10;

    //Xp System
    [Header("Xp System")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Slider easeXpSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    private int baseXpRequirement = 50;
    private int xpRequirement;
    private int maxLevel = 20;
    public int level = 1;
    private int xp = 0;
    private bool gotLastHp = false;

    //Other Projectiles
    [Header("ExplosionBullet")]
    [SerializeField] private GameObject explosionBullet;
    [SerializeField] private GameObject explosionBulletUI;
    [SerializeField] private GameObject explosionIcon;
    [SerializeField] private Transform bulletSpawn;
    public bool explosionBulletEnabled;
    private bool canShootExplosionBullet = true;

    //AcidFullAuto
    [Header("AcidFullAuto")]
    [SerializeField] private GameObject acidFullAutoText;
    public bool AcidFullAutoEnabled = false;

    //Shop System
    [Header("ShopSystem")]
    [SerializeField] private TextMeshProUGUI bloodPointsText;

    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenu;

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
        xpRequirement = baseXpRequirement;
        
    }
    void Update()
    {

        if (Interactable.gamePaused)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Interactable.gamePaused = false;
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
            return;
        }
        if(isDashing)
        {
            return;
        }
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        moveDirection = new Vector2(xInput, yInput).normalized;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(ReloadWeapon());
        }

        //PauseMenu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (explosionBulletEnabled)
            {
                if (canShootExplosionBullet)
                {
                    StartCoroutine(ShootExplosionBullet());
                }
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if(dashEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!isDashing && canDash)
                    {
                        StartCoroutine(Dash());
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        bloodPointsText.text = ("" + bloodPoints);
        //AbilityUI
        explosionBulletUI.SetActive(explosionBulletEnabled);
        explosionIcon.SetActive(canShootExplosionBullet);
        dashUI.SetActive(dashEnabled);
        dashIcon.SetActive(canDash);
        acidFullAutoText.SetActive(AcidFullAutoEnabled);
        if (Interactable.gamePaused || isDashing)
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


        //Xp Bar
        if(level < maxLevel)
        {
            xpSlider.maxValue = xpRequirement;
            easeXpSlider.maxValue = xpRequirement;
            if (xp >= xpRequirement)
            {
                int xpRemaining = xp - xpRequirement;
                xp = 0;
                level += 1;
                GetHealth(1);
                ShopButtons.bulletDamage += 0.01f;
                xpRequirement = baseXpRequirement * 2;
                xp += xpRemaining;
            }
            levelText.text = "Level " + level;
            if (xpSlider.value != xp)
            {
                xpSlider.value = xp;
            }
            if (xpSlider.value != easeXpSlider.value)
            {
                easeXpSlider.value = Mathf.Lerp(easeXpSlider.value, xp, lerpSpeed);
            }
        }
        else
        {
            xpSlider.value = xpSlider.maxValue;
            easeXpSlider.value = easeXpSlider.maxValue;
            if(!gotLastHp)
            {
                gotLastHp = true;
                GetHealth(1);
            }
            levelText.text = "Max Level";
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

    private IEnumerator ShootExplosionBullet()
    {
        canShootExplosionBullet = false;
        GameObject bullet = Instantiate(explosionBullet, bulletSpawn.position, Quaternion.identity);
        bullet.transform.up = -(GetMouseWorldPosition() - bulletSpawn.transform.position);
        yield return new WaitForSeconds(5);
        canShootExplosionBullet = true;
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

    public void GetXp(int xpAmount)
    {
        xp += xpAmount;
    }


    public void GetBlood(int amount)
    {
        bloodPoints += amount;
    }

    public void GetHealth(int amount)
    {
        maxHealth += amount;
        healthSlider.maxValue = maxHealth;
        easeHealthBarSlider.maxValue = maxHealth;
        health += amount;
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
            ShopButtons.bulletDamage += 1 * 0.25f;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        HeadMouthController[] headMouth = FindObjectsOfType<HeadMouthController>();
        for (int i = 0; i < headMouth.Length; i++)
        {
            Physics2D.IgnoreCollision(headMouth[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>());
        }
        CrawlerController[] crawler = FindObjectsOfType<CrawlerController>();
        for (int i = 0; i < crawler.Length; i++)
        {
            Physics2D.IgnoreCollision(crawler[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>());
        }
        EyeController[] eye = FindObjectsOfType<EyeController>();
        for (int i = 0; i < eye.Length; i++)
        {
            Physics2D.IgnoreCollision(eye[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>());
        }
        rb.velocity = moveDirection * dashSpeed;
        Debug.Log(moveDirection + "     " + dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        for (int i = 0; i < headMouth.Length; i++)
        {
            if (headMouth[i] != null)
            {
                Physics2D.IgnoreCollision(headMouth[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>(), false);
            }
        }
        for (int i = 0; i < crawler.Length; i++)
        {
            if (crawler[i] != null)
            {
                Physics2D.IgnoreCollision(crawler[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>(), false);
            }
        }
        for (int i = 0; i < eye.Length; i++)
        {
            if(eye[i] != null)
            {
                Physics2D.IgnoreCollision(eye[i].transform.Find("Shadow").GetComponent<CircleCollider2D>(), transform.Find("Shadow").GetComponent<CircleCollider2D>(), false);
            }
        }
        StartCoroutine(DashCoolDown());
        isDashing = false;
    }

    private IEnumerator DashCoolDown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
