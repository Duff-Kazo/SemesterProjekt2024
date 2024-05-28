using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float knockBackForce;
    public float bonusDamage = 0;
    public float shotGunDamage = 0;
    public bool applyTommyGunDamage = false;
    private bool apply = true;
    [SerializeField] private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;

        if (applyTommyGunDamage)
        {
            if (bonusDamage <= 0.5 && apply)
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
            if(shotGunDamage > 0)
            {
                Rigidbody2D rb = headMouth.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * knockBackForce / 2;
            }
            headMouth.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Crawler"))
        {
            CrawlerController crawler = collision.gameObject.GetComponent<CrawlerController>();
            if (shotGunDamage > 0)
            {
                Rigidbody2D rb = crawler.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * knockBackForce / 2;
            }
            crawler.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Eye"))
        {
            EyeController eye = collision.gameObject.GetComponent<EyeController>();
            if (shotGunDamage > 0)
            {
                Rigidbody2D rb = eye.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * knockBackForce /2;
            }
            eye.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            BossController boss = collision.gameObject.GetComponent<BossController>();
            if (shotGunDamage > 0)
            {
                Rigidbody2D rb = boss.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * knockBackForce / 2;
            }
            boss.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("ShopKeeper"))
        {
            ShopKeeperController shopKeeper = collision.gameObject.GetComponent<ShopKeeperController>();
            if (shotGunDamage > 0)
            {
                Rigidbody2D rb = shopKeeper.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * knockBackForce / 2;
            }
            shopKeeper.TakeDamage(ShopButtons.bulletDamage + bonusDamage + shotGunDamage);
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
