using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShopKeeperController : MonoBehaviour
{
    [Header("Phases")]
    private int phase = 1;
    private bool isAggro = true;
    private Vector2 dirToPlayer;

    [Header("BossBar")]
    [SerializeField] private Image bossBar;
    [SerializeField] private Image easeBossBar;
    [SerializeField] private float MaxHp = 100;
    [SerializeField] private float health = 100;
    private float lerpSpeed = 0.05f;
    private bool isNotInvincible = true;

    [Header("Drops")]
    [SerializeField] private int bloodDropAmount;
    [SerializeField] private GameObject deathParticles;


    //Components
    private SpriteRenderer spriteRenderer;
    private PlayerController player;
    private SoundManager gameManager;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;

    private float scaleX;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        dirToPlayer = player.transform.position - transform.position;
        if (Interactable.gamePaused)
        {
            agent.isStopped = true;
            return;
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

    private void FixedUpdate()
    {
        UpdateHealthBar();
        if (Interactable.gamePaused)
        {
            agent.isStopped = true;
            return;
        }

        if(isAggro)
        {
            if (phase == 1)
            {
                StartCoroutine(Phase1());
            }
        }

        if(agent.velocity.magnitude == 0)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }


    }

    private IEnumerator Phase1()
    {
        if (dirToPlayer.magnitude > 3)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector2.zero;
        }
        yield return null;
    }

    public void TakeDamage(float damage)
    {
        if (isNotInvincible)
        {
            if (health > 0)
            {
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
        }
    }

    private IEnumerator DamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void UpdateHealthBar()
    {
        if (bossBar.fillAmount != health)
        {
            bossBar.fillAmount = health / MaxHp;
        }

        if (bossBar.fillAmount != easeBossBar.fillAmount)
        {
            easeBossBar.fillAmount = Mathf.Lerp(easeBossBar.fillAmount, health / MaxHp, lerpSpeed);
        }
    }

    private void Die()
    {
        player.GetBlood(bloodDropAmount);
        if (player.monsterState == 3)
        {
            player.GetXp(1850);
        }
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
