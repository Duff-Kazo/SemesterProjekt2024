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

    [Header("HealthBar")]
    [SerializeField] private GameObject healthBarCanvas;
    private FloatingHealthBar healthBar;

    //Components
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
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
    }

    private void Update()
    {
        if (Interactable.inShop)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
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
        healthBar.updateHealthBar(health, maxHealth);
    }

    private void FixedUpdate()
    {
        if (Interactable.inShop)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        if (isAggro)
        {
            dirToPlayer = player.transform.position - bulletSpawn.transform.position;
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

    private void Shoot()
    {
        if (playerInSight)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.transform.up = -dirToPlayer;
        }
    }

    private void Die()
    {
        player.GetBlood(bloodDropAmount);
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
        if (health > 0)
        {
            StartCoroutine(DamageAnimation());
            health -= damage;
        }
        else if (health <= 0)
        {
            Die();
        }
    }
}
