using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bonusDamage = 0;
    public float shotGunDamage = 0;
    public bool applyTommyGunDamage = false;
    private bool apply = true;
    [SerializeField] private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.up * speed;

        if(applyTommyGunDamage)
        {
            if(bonusDamage <= 1 && apply)
            {
                StartCoroutine(ApplyBonusDamage());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("HeadMouth"))
        {
            HeadMouthController headMouth = collision.gameObject.GetComponent<HeadMouthController>();
            headMouth.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Crawler"))
        {
            CrawlerController crawler = collision.gameObject.GetComponent<CrawlerController>();
            crawler.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Eye"))
        {
            EyeController eye = collision.gameObject.GetComponent<EyeController>();
            eye.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            BossController boss = collision.gameObject.GetComponent<BossController>();
            boss.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyBonusDamage()
    {
        apply = false;
        bonusDamage += 0.2f;
        yield return new WaitForSeconds(0.25f);
        apply = true;
    }
}
