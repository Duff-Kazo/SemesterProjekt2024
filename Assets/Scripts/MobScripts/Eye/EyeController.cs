using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyeController : MonoBehaviour
{
    public bool isAggro = false;
    private PlayerController player;
    private float scaleX;
    private Vector2 dirToPlayer;
    private float laserCounter = 0;
    private bool playerInSight = false;
    private bool isShooting = false;
    [SerializeField] private float health = 5;
    [SerializeField] private float laserCoolDown;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int bloodDropAmount;

    //Components
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform laserOrigin;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        scaleX = transform.localScale.x;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
            //lineRenderer.enabled = true;
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
        if (Interactable.inShop || !isAggro)
        {
            agent.isStopped = true;
            return;
        }

        if (!isShooting)
        {
            agent.isStopped = false;
            dirToPlayer = player.transform.position - bulletSpawn.transform.position;
            if (dirToPlayer.magnitude > 5)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.isStopped = true;
            }


            if (laserCounter < laserCoolDown)
            {
                laserCounter += Time.deltaTime;
            }
            else
            {
                if (!isShooting)
                {
                    laserCounter = 0;
                    StartCoroutine(Shoot());
                }
            }

        }
        else
        {
            agent.isStopped = true;
        }

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
        }
        else if (dirToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        Transform playerPos = player.transform;
        yield return new WaitForSeconds(2f);
        lineRenderer.enabled = true;

        Vector2 direction = playerPos.position - bulletSpawn.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawn.position, direction, 15, layerMask);
        if (hit)
        {
            lineRenderer.SetPosition(0, laserOrigin.transform.position);
            lineRenderer.SetPosition(1, hit.transform.position);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                player.TakeDamage(3f);
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
