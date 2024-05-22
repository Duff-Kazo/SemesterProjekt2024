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
    [SerializeField] private float reloadTime = 2;
    public bool isReloading = false;
    private PlayerAcidBall playerAcidBall;
    public int bulletCount = 0;
    [SerializeField] public int maxMagazines = 4;
    public int maxBullets = 16;
    public int magazinesCount;
    public int magazineBullets;

    [Header("Weapons")]
    public static bool gunEquiped = true;
    public static bool MP40Equiped = false;
    public static bool tommyGunEquiped = false;
    public static bool shotgunEquiped = false;
    private MP40Controll mp40;
    private ShotGunController shotGun;
    private TommyGunController tommyGun;
    private PlayerWeaponAim pistol;
    private MP40Aim mp40Aim;
    private ShotGunAim shotGunAim;
    private TommyGunAim tommyGunAim;
    private Aim playerWeaponAim;
    [SerializeField] private GameObject shotgunGraphic;
    [SerializeField] private GameObject pistolGraphic;
    [SerializeField] private GameObject tommygunGraphic;
    [SerializeField] private GameObject mp40Graphic;

    [SerializeField] private GameObject shotGunUI, shotGunUIBackground;
    [SerializeField] private GameObject pistolUI, pistolUIBackground;
    [SerializeField] private GameObject tommyGunUI, tommyGunUIBackground;
    [SerializeField] private GameObject mp40UI, mp40UIBackground;

    //Shield
    [Header("Shield")]
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider easeShieldSlider;
    [SerializeField] private GameObject shieldGraphic;
    [SerializeField] private GameObject shieldBackground;
    public bool shieldEnabled = false;
    private float shieldMaxHealth = 3;
    private float shieldCurrentHealth;
    private bool isRegenerating = false;
    private bool breakShield = false;

    //Dash
    [Header("Dash")]
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashLength;
    [SerializeField] private float dashCount = 3;
    [SerializeField] private float baseDashCount = 3;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private GameObject dashUI;
    [SerializeField] private GameObject dashIcon;
    [SerializeField] private TextMeshProUGUI dashCoolDownText;
    private bool canDash = true;
    private Vector2 moveDirection;
    public bool dashEnabled;
    private float dashTimer = 0;

    //MonsterItems
    public static bool eyesActivated = false;
    public static bool shieldActivated = false;
    public static bool orbsActivated = false;
    public static bool plagueActivated = false;


    //Animations
    [Header("Animations")]
    [SerializeField] public int monsterState = 0;
    private int tempMonsterState = 0;
    [SerializeField] private TextMeshProUGUI currentMonsterState;
    [SerializeField] private TextMeshProUGUI newMonsterState;
    [SerializeField] private GameObject monsterStatePanel;
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
    [SerializeField] private GameObject currentHealthTextInstance;
    [SerializeField] private GameObject maxHealthTextInstance;
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
    [SerializeField] private TextMeshProUGUI explosionBulletCoolDownText;
    public bool explosionBulletEnabled;
    private bool canShootExplosionBullet = true;

    [SerializeField] private float explosionBulletCount = 5;
    [SerializeField] private float baseExplosionBulletCount = 5;
    private float explosionBulletTimer = 0;

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
    [SerializeField] private AudioSource reloadTommygun;
    [SerializeField] private AudioSource reloadMP40;
    [SerializeField] private AudioSource reloadShotgun;
    private bool shotgun;
    private bool mp;
    private bool tommy;
    private bool gun;

    //Map
    [Header("Map")]
    [SerializeField] private GameObject largeMap;
    [SerializeField] private GameObject miniMap;
    private bool largeMapOpen = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        scaleX = transform.localScale.x;
        health = maxHealth;
        bulletCount = maxBullets;
        magazinesCount = maxMagazines;
        magazineBullets = magazinesCount * maxBullets;
        acidUI.SetActive(false);
        bulletUI.SetActive(true);
        xpRequirement = baseXpRequirement;
        largeMapOpen = false;
        pistol = GetComponent<PlayerWeaponAim>();
        shotGun = GetComponent<ShotGunController>();
        tommyGun = GetComponent<TommyGunController>();
        mp40 = GetComponent<MP40Controll>();
        playerAcidBall = FindObjectOfType<PlayerAcidBall>();
        playerWeaponAim = FindObjectOfType<Aim>();
        shotGunAim = FindObjectOfType<ShotGunAim>();
        tommyGunAim = FindObjectOfType<TommyGunAim>();
        mp40Aim = FindObjectOfType<MP40Aim>();
        playerAcidBall.enabled = false;
        gun = true;
        shotgun = false;
        tommy = false;
        mp = false;
        gunEquiped = false;
        pistol.enabled = true;
        shotGun.enabled = false;
        tommyGun.enabled = false;
        mp40.enabled = false;
        pistolGraphic.SetActive(true);
        shotgunGraphic.SetActive(false);
        tommygunGraphic.SetActive(false);
        mp40Graphic.SetActive(false);
        playerWeaponAim.enabled = true;
        shotGunAim.enabled = false;
        tommyGunAim.enabled = false;
        mp40Aim.enabled = false;
        shieldCurrentHealth = shieldMaxHealth;
        shieldGraphic.SetActive(false);
        CloseLargeMap();
        monsterStatePanel.SetActive(false);
    }
    void Update()
    {
        if(playerWeaponAim != null && shotGunAim != null && tommyGunAim != null && mp40Aim != null)
        {
            SelectWeapon();
        }
        else
        {
            Debug.Log("ICH BRING MICH UM");
        }
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
            DashCoolDown();
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

        if (canShootExplosionBullet)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (explosionBulletEnabled)
                {
                    ShootExplosionBullet();
                }
            }
        }
        else
        {
            ExplosionBulletCoolDown();
        }
        if(canDash)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (dashEnabled)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!isDashing)
                        {
                            StartCoroutine(Dash());
                        }
                    }
                }
            }
        }
        else
        {
            DashCoolDown();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(!largeMapOpen)
            {
                OpenLargeMap();
            }
            else
            {
                CloseLargeMap();
            }
        }
    }

    private void FixedUpdate()
    {
        bloodPointsText.text = ("" + bloodPoints);
        dashUI.SetActive(dashEnabled);
        explosionBulletUI.SetActive(explosionBulletEnabled);

        //AbilityUI
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
            pistol.enabled = false;
            shotGun.enabled = false;
            tommyGun.enabled = false;
            mp40.enabled = false;
            pistolGraphic.SetActive(false);
            shotgunGraphic.SetActive(false);
            tommygunGraphic.SetActive(false);
            mp40Graphic.SetActive(false);
            
            playerAcidBall.enabled = true;
            bulletUI.SetActive(false);
            acidUI.SetActive(true);
        }

        //HEALTHBAR
        if(health > maxHealth)
        {
            health = maxHealth;
        }
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

        //ShieldBar
        if(shieldEnabled)
        {
            if (shieldCurrentHealth > 0)
            {
                currentHealthTextInstance.SetActive(false);
                maxHealthTextInstance.SetActive(false);
                shieldGraphic.SetActive(true);
                shieldBackground.SetActive(true);
                shieldGraphic.transform.localScale = new Vector3(0.5f * shieldCurrentHealth, 0.5f * shieldCurrentHealth, 0);
            }
            else
            {
                currentHealthTextInstance.SetActive(true);
                maxHealthTextInstance.SetActive(true);
                shieldGraphic.SetActive(false);
                shieldBackground.SetActive(false);
            }
            if (shieldSlider.value != shieldCurrentHealth)
            {
                shieldSlider.value = shieldCurrentHealth;
            }
            if (shieldSlider.value != easeShieldSlider.value)
            {
                easeShieldSlider.value = Mathf.Lerp(easeShieldSlider.value, shieldCurrentHealth, lerpSpeed);
            }
        }
        else
        {
            shieldSlider.value = 0;
            easeShieldSlider.value = 0;
        }



        

        //RegenerateShield
        if(shieldCurrentHealth < shieldMaxHealth)
        {
            if(!isRegenerating)
            {
                StartCoroutine(RegenerateShield());
            }
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
                GetMaxHealth(1);
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
                GetMaxHealth(1);
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
        if(shotgun)
        {
            if(bulletCount < 4 && !isReloading)
            {
                StartCoroutine(ReloadWeapon());
            }
        }
        if(bulletCount <= 0 && !isReloading)
        {
            StartCoroutine(ReloadWeapon());
        }
    }

    public void ChangeMonsterState()
    {
        if(monsterState < 3)
        {
            tempMonsterState = monsterState;
            monsterState++;
            StartCoroutine(MonsterStateAnimation());
        }
    }

    private IEnumerator MonsterStateAnimation()
    {
        monsterStatePanel.SetActive(true);
        Animator monsterStateAnimator = monsterStatePanel.GetComponent<Animator>();
        currentMonsterState.text = tempMonsterState.ToString();
        newMonsterState.text = monsterState.ToString();
        monsterStateAnimator.SetTrigger("ChangeMonsterState");
        CinemachineShake.instance.ShakeCamera(4, 1.99f);
        yield return new WaitForSeconds(1.99f);
        monsterStatePanel.SetActive(false);
    }

    private IEnumerator RegenerateShield()
    {
        isRegenerating = true;
        for(int i = 0; i < shieldMaxHealth; i++)
        {
            if(breakShield)
            {
                breakShield = false;
                break;
            }
            yield return new WaitForSeconds(3f);
            if (breakShield)
            {
                breakShield = false;
                break;
            }
            if (shieldCurrentHealth < shieldMaxHealth)
            {
                shieldCurrentHealth++;
            }
        }
        isRegenerating = false;
    }

    public void TakeDamage(float damage)
    {
        if(shieldEnabled && shieldCurrentHealth > 0)
        {
            shieldCurrentHealth--;
            breakShield = true;
        }
        else
        {
            breakShield = true;
            damageSound.Play();
            StartCoroutine(DamageAnimation());
            Instantiate(blood, transform.position, Quaternion.identity);
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator ReloadWeapon()
    {
        if(pistol.enabled || shotGun.enabled || tommyGun.enabled || mp40.enabled)
        {
            if (magazineBullets > 0 && bulletCount != maxBullets)
            {
                magazinesCount -= 1;
                isReloading = true;
                pistol.canShoot = false;
                shotGun.canShoot = false;
                tommyGun.canShoot = false;
                mp40.canShoot = false;
                reloadAnimation.value = reloadAnimation.minValue;
                if(gun)
                {
                    reload.Play();
                    pistolUI.SetActive(false);
                }
                else if(shotgun)
                {
                    reloadShotgun.Play();
                    shotGunUI.SetActive(false);
                }
                else if(tommy)
                {
                    reloadTommygun.Play();
                    tommyGunUI.SetActive(false);
                }
                else if(mp)
                {
                    reloadMP40.Play();
                    mp40UI.SetActive(false);
                }
                yield return new WaitForSeconds(reloadTime);
                if (gun)
                {
                    loaded.Play();
                    pistolUI.SetActive(true);
                }
                else if (shotgun)
                {
                    shotGunUI.SetActive(true);
                }
                else if (tommy)
                {
                    tommyGunUI.SetActive(true);
                }
                else if (mp)
                {
                    mp40UI.SetActive(true);
                }
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
                pistol.canShoot = true;
                shotGun.canShoot = true;
                tommyGun.canShoot = true;
                mp40.canShoot = true;
                isReloading = false;
            }
        }

        //If weapon Enabled war da auch noch warum auch immer also so drum herum um alles
    }

    private void ShootExplosionBullet()
    {
        GameObject bullet = Instantiate(explosionBullet, bulletSpawn.position, Quaternion.identity);
        bullet.transform.up = -(GetMouseWorldPosition() - bulletSpawn.transform.position);
        canShootExplosionBullet = false;
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

    public void GetMaxHealth(int amount)
    {
        maxHealth += amount;
        healthSlider.maxValue = maxHealth;
        easeHealthBarSlider.maxValue = maxHealth;
        health += amount;
    }

    public void GetHealth(int amount)
    {
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
            MP40Equiped = true;
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
        else if(itemName == "Shotgun")
        {
            shotgunEquiped = true;
        }
        else if(itemName == "Tommygun")
        {
            tommyGunEquiped = true;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        HeadMouthController[] headMouth = FindObjectsOfType<HeadMouthController>();
        for (int i = 0; i < headMouth.Length; i++)
        {
            Physics2D.IgnoreCollision(headMouth[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>());
        }
        CrawlerController[] crawler = FindObjectsOfType<CrawlerController>();
        for (int i = 0; i < crawler.Length; i++)
        {
            Physics2D.IgnoreCollision(crawler[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>());
        }
        EyeController[] eye = FindObjectsOfType<EyeController>();
        for (int i = 0; i < eye.Length; i++)
        {
            Physics2D.IgnoreCollision(eye[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>());
        }
        rb.velocity = moveDirection * dashSpeed;
        Debug.Log(moveDirection + "     " + dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        for (int i = 0; i < headMouth.Length; i++)
        {
            if (headMouth[i] != null)
            {
                Physics2D.IgnoreCollision(headMouth[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>(), false);
            }
        }
        for (int i = 0; i < crawler.Length; i++)
        {
            if (crawler[i] != null)
            {
                Physics2D.IgnoreCollision(crawler[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>(), false);
            }
        }
        for (int i = 0; i < eye.Length; i++)
        {
            if(eye[i] != null)
            {
                Physics2D.IgnoreCollision(eye[i].transform.Find("Shadow").GetComponent<BoxCollider2D>(), transform.Find("Shadow").GetComponent<CapsuleCollider2D>(), false);
            }
        }
        canDash = false;
        isDashing = false;
    }

    private void DashCoolDown()
    {
        canDash = false;
        if(dashCount > dashTimer)
        {
            dashCount -= Time.deltaTime;
            dashCoolDownText.text = dashCount.ToString("0");
            canDash = false;
        }
        else
        {
            dashCount = baseDashCount;
            canDash = true;
            dashCoolDownText.text = "";
        }
    }

    private void SelectWeapon()
    {
        if(gunEquiped)
        {
            gunEquiped = false;
            gun = true;
            shotgun = false;
            tommy = false;
            mp = false;
            pistol.enabled = true;
            shotGun.enabled = false;
            tommyGun.enabled = false;
            mp40.enabled = false;

            pistolGraphic.SetActive(true);
            shotgunGraphic.SetActive(false);
            tommygunGraphic.SetActive(false);
            mp40Graphic.SetActive(false);

            pistolUI.SetActive(true);
            pistolUIBackground.SetActive(true);
            tommyGunUI.SetActive(false);
            tommyGunUIBackground.SetActive(false);
            shotGunUI.SetActive(false);
            shotGunUIBackground.SetActive(false);
            mp40UI.SetActive(false);
            mp40UIBackground.SetActive(false);

            playerWeaponAim.enabled = true;
            shotGunAim.enabled = false;
            tommyGunAim.enabled = false;
            mp40Aim.enabled = false;
        }
        if (shotgunEquiped)
        {
            shotgunEquiped = false;
            shotgun = true;
            gun = false;
            tommy = false;
            mp = false;
            pistol.enabled = false;
            shotGun.enabled = true;
            tommyGun.enabled = false;
            mp40.enabled = false;

            pistolGraphic.SetActive(false);
            shotgunGraphic.SetActive(true);
            tommygunGraphic.SetActive(false);
            mp40Graphic.SetActive(false);

            pistolUI.SetActive(false);
            pistolUIBackground.SetActive(false);
            tommyGunUI.SetActive(false);
            tommyGunUIBackground.SetActive(false);
            shotGunUI.SetActive(true);
            shotGunUIBackground.SetActive(true);
            mp40UI.SetActive(false);
            mp40UIBackground.SetActive(false);

            playerWeaponAim.enabled = false;
            shotGunAim.enabled = true;
            tommyGunAim.enabled = false;
            mp40Aim.enabled = false;
        }
        if (tommyGunEquiped)
        {
            tommyGunEquiped = false;
            tommy = true;
            gun = false;
            shotgun = false;
            mp = false;
            pistol.enabled = false;
            shotGun.enabled = false;
            tommyGun.enabled = true;
            mp40.enabled = false;

            pistolGraphic.SetActive(false);
            shotgunGraphic.SetActive(false);
            tommygunGraphic.SetActive(true);
            mp40Graphic.SetActive(false);

            pistolUI.SetActive(false);
            pistolUIBackground.SetActive(false);
            tommyGunUI.SetActive(true);
            tommyGunUIBackground.SetActive(true);
            shotGunUI.SetActive(false);
            shotGunUIBackground.SetActive(false);
            mp40UI.SetActive(false);
            mp40UIBackground.SetActive(false);

            playerWeaponAim.enabled = false;
            shotGunAim.enabled = false;
            tommyGunAim.enabled = true;
            mp40Aim.enabled = false;
        }
        if (MP40Equiped)
        {
            MP40Equiped = false;
            mp = true;
            tommy = false;
            gun = false;
            shotgun = false;
            pistol.enabled = false;
            shotGun.enabled = false;
            tommyGun.enabled = false;
            mp40.enabled = true;


            pistolGraphic.SetActive(false);
            shotgunGraphic.SetActive(false);
            tommygunGraphic.SetActive(false);
            mp40Graphic.SetActive(true);

            pistolUI.SetActive(false);
            pistolUIBackground.SetActive(false);
            tommyGunUI.SetActive(false);
            tommyGunUIBackground.SetActive(false);
            shotGunUI.SetActive(false);
            shotGunUIBackground.SetActive(false);
            mp40UI.SetActive(true);
            mp40UIBackground.SetActive(true);

            playerWeaponAim.enabled = false;
            shotGunAim.enabled = false;
            tommyGunAim.enabled = false;
            mp40Aim.enabled = true;
        }
    }

    private void ExplosionBulletCoolDown()
    {
        canShootExplosionBullet = false;
        if (explosionBulletCount > explosionBulletTimer)
        {
            explosionBulletCount -= Time.deltaTime;
            explosionBulletCoolDownText.text = explosionBulletCount.ToString("0");
            canShootExplosionBullet = false;
        }
        else
        {
            explosionBulletCount = baseExplosionBulletCount;
            canShootExplosionBullet = true;
            explosionBulletCoolDownText.text = "";
        }
    }

    private void OpenLargeMap()
    {
        largeMapOpen = true;
        miniMap.SetActive(false);
        largeMap.SetActive(true);
    }

    private void CloseLargeMap()
    {
        largeMapOpen = false;
        miniMap.SetActive(true);
        largeMap.SetActive(false);
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