using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAcidBallPrefab : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0, 0, 1), -270);
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
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
            headMouth.TakeDamage(ShopButtons.bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Crawler"))
        {
            CrawlerController crawler = collision.gameObject.GetComponent<CrawlerController>();
            crawler.TakeDamage(ShopButtons.bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Eye"))
        {
            EyeController eye = collision.gameObject.GetComponent<EyeController>();
            eye.TakeDamage(ShopButtons.bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            BossController boss = collision.gameObject.GetComponent<BossController>();
            boss.TakeDamage(ShopButtons.bulletDamage);
            Destroy(gameObject);
        }
    }
}
