using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EyeController : MonoBehaviour
{
    public bool isAggro = false;
    private PlayerController player;
    private float scaleX;
    private float healthBarScalex;
    private float healthBarScaley;
    private Vector2 dirToPlayer;
    private float laserCounter = 0;
    private bool playerInSight = false;
    private bool isShooting = false;
    [Header("Health")]
    [SerializeField] private float health = 5;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private int bloodDropAmount;
    [SerializeField] private GameObject bloodParticles;

    [Header("HealthBar")]
    [SerializeField] private GameObject healthBarCanvas;
    private FloatingHealthBar healthBar;

    [Header("Laser")]
    [SerializeField] private float laserCoolDown;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask layerMaskVisor;
    [SerializeField] private float damage;

    [Header("Sounds")]
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource chargeSound;
    [SerializeField] private AudioSource hitSound;

    //Components
    [Header("Components")]
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SoundManager gameManager;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private GameObject plagueZone;
    [SerializeField] private GameObject corpse;

    //ItemEffects
    [Header("ItemEffects")]
    [SerializeField] private new GameObject light;
    [SerializeField] private GameObject mapIcon;
    private bool plagueActivated = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        scaleX = transform.localScale.x;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lineRenderer.enabled = false;
        isAggro = false;
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBarScalex = scaleX * 0.5f;
        healthBarScaley = transform.localScale.y * 0.5f;
        gameManager = FindObjectOfType<SoundManager>();
        plagueZone.SetActive(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
        healthBar.updateHealthBar(health, maxHealth);
        //light.SetActive(PlayerController.eyesActivated);
        mapIcon.SetActive(PlayerController.eyesActivated);
        if (Interactable.gamePaused || isShooting || !isAggro)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        dirToPlayer = player.transform.position - bulletSpawn.transform.position;
        Vector2 direction = player.transform.position - bulletSpawn.position;
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawn.position, direction, 15, layerMask);
        if (hit)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                playerInSight = true;
            }
            else
            {
                playerInSight = false;
            }
        }
        else
        {
            //lineRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (Interactable.gamePaused)
        {
            agent.isStopped = true;
            agent.velocity = Vector2.zero;
            return;
        }
        if (isAggro)
        {
            if (playerInSight)
            {
                if (dirToPlayer.magnitude > 5)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                    agent.velocity = Vector2.zero;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }

            if (!isShooting && playerInSight)
            {
                if (laserCounter < laserCoolDown)
                {
                    laserCounter += Time.deltaTime;
                }
                else
                {
                    laserCounter = 0;
                    StartCoroutine(Shoot());
                }
            }
        }

        healthBarCanvas.SetActive(isAggro);

        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            healthBarCanvas.transform.localScale = new Vector3(healthBarScalex, healthBarScaley, transform.localScale.z);
        }
        else if (dirToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
            healthBarCanvas.transform.localScale = new Vector3(-healthBarScalex, healthBarScaley, transform.localScale.z);
        }
        else if(dirToPlayer.x == 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            healthBarCanvas.transform.localScale = new Vector3(healthBarScalex, healthBarScaley, transform.localScale.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HeadMouthPlagueZone") || (collision.gameObject.CompareTag("CrawlerPlagueZone")))
        {
            if (PlayerController.plagueActivated && !plagueActivated)
            {
                plagueActivated = true;
                StartCoroutine(PlagueEffect());
            }
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        rb.velocity = Vector3.zero;
        agent.velocity = Vector2.zero;
        agent.ResetPath();
        Vector3 playerPos = player.transform.position;
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.025f;
        lineRenderer.endWidth = 0.025f;

        chargeSound.Play();
        Vector3 direction = playerPos - bulletSpawn.transform.position;
        RaycastHit2D hit1 = Physics2D.Raycast(bulletSpawn.position, direction, 999, layerMaskVisor);
        if(hit1)
        {
            lineRenderer.SetPosition(0, laserOrigin.transform.position);
            lineRenderer.SetPosition(1, hit1.point);
        }
        yield return new WaitForSeconds(0.5f);
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawn.position, direction, 15, layerMask);
        if (hit)
        {
            shootSound.Play();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, laserOrigin.transform.position);
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                player.TakeDamage(damage);
                Debug.Log("LaserHit");
            }

        }
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
        isShooting = false;
    }

    private void Die()
    {
        player.GetBlood(bloodDropAmount);
        if (player.monsterState == 3)
        {
            player.GetXp(25);
        }
        Instantiate(bloodParticles, transform.position, Quaternion.identity);
        GameObject spawned = Instantiate(corpse, transform.position, Quaternion.identity);
        spawned.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    private IEnumerator DamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void TakeDamage(float damage)
    {
        isAggro = true;
        if (health > 0)
        {
            hitSound.Play();
            StartCoroutine(DamageAnimation());
            health -= damage;
            if (health <= 0)
            {
                gameManager.PlayEnemyDeathSound();
                Die();
            }
        }
        else if (health <= 0)
        {
            gameManager.PlayEnemyDeathSound();
            Die();
        }

        if (PlayerController.plagueActivated &&! plagueActivated)
        {
            plagueActivated = true;
            StartCoroutine(PlagueEffect());
        }
    }

    private IEnumerator PlagueEffect()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1.5f);
            if (health > 1)
            {
                TakeDamage(1f);
                plagueZone.SetActive(true);
            }
            else
            {
                plagueZone.SetActive(false);
            }
        }
    }
}
