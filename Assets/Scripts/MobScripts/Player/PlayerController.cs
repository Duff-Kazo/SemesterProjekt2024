using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using Cinemachine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

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
    [SerializeField] private float speed;

    //Animations
    private int monsterState = 0;
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
    [SerializeField] private float maxHealth = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        scaleX = transform.localScale.x;
        health = maxHealth;
    }
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
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
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if(healthSlider.value != easeHealthBarSlider.value)
        {
            easeHealthBarSlider.value = Mathf.Lerp(easeHealthBarSlider.value, health, lerpSpeed);
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

    private IEnumerator DamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
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
}
