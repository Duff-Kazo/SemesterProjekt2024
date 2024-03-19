using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.up * speed;
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
    }
}
