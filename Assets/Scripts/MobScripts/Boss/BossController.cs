using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public bool isAggro = false;
    private PlayerController player;
    private float scaleX;
    private Vector2 dirToPlayer;
    private float shootCounter = 0;
    private bool playerInSight = false;
    [SerializeField] private float bulletCoolDown;
    [SerializeField] private int bloodDropAmount;
    [SerializeField] private GameObject deathParticles;

    [Header("BossBar")]
    [SerializeField] private Image bossBar;
    [SerializeField] private Image easeBossBar;
    [SerializeField] private float MaxHp = 100;
    [SerializeField] private float health = 100;
    private float lerpSpeed = 0.05f;


    [Header("LaserAttack")]
    [SerializeField] private Transform laser1;
    [SerializeField] private Transform laser2;
    [SerializeField] private Transform laser3;
    [SerializeField] private Transform laser4;
    [SerializeField] private Transform laser5;
    [SerializeField] private Transform laser6;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask layerMaskVisor;
    [SerializeField] private float damage = 3;

    [Header("SpawnEnemies")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject headMouthPrefab;
    [SerializeField] private GameObject crawlerPrefab;
    [SerializeField] private GameObject eyePrefab;
    [SerializeField] private float spawnPhaseTime = 10;
    [SerializeField] private float laserPhaseTime = 10;
    private bool isSpawningEnemies = false;
    private bool isLaserAttacking = false;
    private float spawnPhaseCounter = 0;
    private float laserPhaseCounter = 0;

    private bool phase1 = true;

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
        StartCoroutine(WaitForStart());
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
    }

    private void FixedUpdate()
    {
        if (isAggro)
        {
            agent.isStopped = false;
            if (phase1)
            {
                Phase1();
            }
            else
            {
                Phase2();
            }
        }
        if (bossBar.fillAmount != health)
        {
            bossBar.fillAmount = health / MaxHp;
        }

        if (bossBar.fillAmount != easeBossBar.fillAmount)
        {
            easeBossBar.fillAmount = Mathf.Lerp(easeBossBar.fillAmount, health / MaxHp, lerpSpeed);
        }
        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (dirToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }

        if (health <= MaxHp / 2)
        {
            phase1 = false;
        }

        if (Interactable.inShop)
        {
            agent.isStopped = true;
            return;
        }
    }

    private void Phase2()
    {
        if (isSpawningEnemies || isLaserAttacking)
        {
            return;
        }
        bulletCoolDown = 0.25f;
        dirToPlayer = player.transform.position - bulletSpawn.transform.position;
        if (dirToPlayer.magnitude > 2)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector2.zero;
        }
        if (shootCounter < bulletCoolDown)
        {
            shootCounter += Time.deltaTime;
        }
        else
        {
            shootCounter = 0;
            Shoot();
        }
        if (laserPhaseCounter < laserPhaseTime)
        {
            laserPhaseCounter += Time.deltaTime;
        }
        else
        {
            laserPhaseCounter = 0;
            StartCoroutine(LaserAttack());
        }
    }


    private void Phase1()
    {
        if (isSpawningEnemies || isLaserAttacking)
        {
            return;
        }
        dirToPlayer = player.transform.position - bulletSpawn.transform.position;
        if (dirToPlayer.magnitude > 5)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector2.zero;
        }
        if (shootCounter < bulletCoolDown)
        {
            shootCounter += Time.deltaTime;
        }
        else
        {
            shootCounter = 0;
            Shoot();
        }
        if (spawnPhaseCounter < spawnPhaseTime)
        {
            spawnPhaseCounter += Time.deltaTime;
        }
        else
        {
            spawnPhaseCounter = 0;
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1f);
        isAggro = true;
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
        Instantiate(deathParticles, transform.position, Quaternion.identity);
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


    private IEnumerator SpawnEnemies()
    {
        isSpawningEnemies = true;
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
        agent.SetDestination(centerPoint.position);
        yield return new WaitForSeconds(2f);
        float enemyCount = 0;
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                float randomEnemyIndex = Random.Range(1, 3);
                if (randomEnemyIndex == 1)
                {
                    Instantiate(headMouthPrefab, spawnPoint.position, Quaternion.identity);
                }
                else if (randomEnemyIndex == 2)
                {
                    Instantiate(crawlerPrefab, spawnPoint.position, Quaternion.identity);
                }
                else if (randomEnemyIndex == 3)
                {
                    Instantiate(eyePrefab, spawnPoint.position, Quaternion.identity);
                }
                enemyCount++;
                yield return new WaitForSeconds(1f);
            }
            if(enemyCount >= 3)
            {
                break;
            }
        }
        
        isSpawningEnemies =false;
    }

    private IEnumerator LaserAttack()
    {
        isLaserAttacking = true;
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
        agent.SetDestination(centerPoint.position);
        yield return new WaitForSeconds(2f);

        for (int i = 1; i <= 6; i++)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FireSingleLaser(i));
            Debug.Log(i);
        }
        yield return new WaitForSeconds(1f);
        isLaserAttacking = false;
    }

    IEnumerator FireSingleLaser(int LaserIndex)
    {
        Transform selectedLaser = laser1;
        if(LaserIndex == 1)
        {
            selectedLaser = laser1;
        }
        else if(LaserIndex == 2)
        {
            selectedLaser = laser2;
        }
        else if (LaserIndex == 3)
        {
            selectedLaser = laser3;
        }
        else if (LaserIndex == 4)
        {
            selectedLaser = laser4;
        }
        else if (LaserIndex == 5)
        {
            selectedLaser = laser5;
        }
        else if (LaserIndex == 6)
        {
            selectedLaser = laser6;
        }

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.025f;
        lineRenderer.endWidth = 0.025f;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = playerPos - selectedLaser.transform.position;
        RaycastHit2D hit1 = Physics2D.Raycast(selectedLaser.position, direction, 999, layerMaskVisor);
        if (hit1)
        {
            lineRenderer.SetPosition(0, selectedLaser.transform.position);
            lineRenderer.SetPosition(1, hit1.point);
        }
        yield return new WaitForSeconds(0.5f);
        RaycastHit2D hit = Physics2D.Raycast(selectedLaser.position, direction, 15, layerMask);
        if (hit)
        {
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, selectedLaser.transform.position);
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                player.TakeDamage(damage);
                Debug.Log("LaserHit");
            }
        }
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}
