using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerController : MonoBehaviour
{
    public bool isAggro = false;
    private PlayerController player;
    private float scaleX;
    private Vector2 dirToPlayer;
    private float counter = 0;
    private bool playerInSight = false;
    [SerializeField] private float bulletCoolDown;
    [SerializeField] private LayerMask layerMask;

    [Header("Health")]
    [SerializeField] private float health = 5;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private int bloodDropAmount;
    [SerializeField] private GameObject bloodParticles;

    [Header("HealthBar")]
    [SerializeField] private GameObject healthBarCanvas;
    private FloatingHealthBar healthBar;

    [Header("Sounds")]
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource hitSound;

    //Components
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SoundManager gameManager;
    [Header("Shooting")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject plagueZone;
    [SerializeField] private GameObject corpse;

    //ItemEffects
    [Header("ItemEffects")]
    [SerializeField] private new GameObject light;
    [SerializeField] private GameObject mapIcon;
    private bool plagueActivated = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        scaleX = transform.localScale.x;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        gameManager = FindObjectOfType<SoundManager>();
        plagueZone.SetActive(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
        healthBar.updateHealthBar(health, maxHealth);
        //light.SetActive(PlayerController.eyesActivated);
        mapIcon.SetActive(PlayerController.eyesActivated);
        if (Interactable.gamePaused)
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
    }

    private void FixedUpdate()
    {
        if (Interactable.gamePaused)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
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


            if (counter < bulletCoolDown)
            {
                counter += Time.deltaTime;
            }
            else
            {
                counter = 0;
                Shoot();
            }

        }

        healthBarCanvas.SetActive(isAggro);

        if (agent.velocity.magnitude > 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            healthBarCanvas.transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (dirToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
            healthBarCanvas.transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HeadMouthPlagueZone") || (collision.gameObject.CompareTag("EyePlagueZone")))
        {
            if (PlayerController.plagueActivated && !plagueActivated)
            {
                plagueActivated = true;
                StartCoroutine(PlagueEffect());
            }
        }
    }

    private void Shoot()
    {
        if (playerInSight)
        {
            shootSound.Play();
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.transform.up = -dirToPlayer;
        }
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

        if (PlayerController.plagueActivated && !plagueActivated)
        {
            plagueActivated = true;
            StartCoroutine(PlagueEffect());
        }
    }
    private IEnumerator PlagueEffect()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1f);
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
