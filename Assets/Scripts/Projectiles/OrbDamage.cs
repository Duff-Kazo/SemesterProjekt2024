using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("HeadMouth"))
        {
            HeadMouthController headMouth = collision.gameObject.GetComponent<HeadMouthController>();
            headMouth.TakeDamage(1f);
        }
        if (collision.gameObject.CompareTag("Crawler"))
        {
            CrawlerController crawler = collision.gameObject.GetComponent<CrawlerController>();
            crawler.TakeDamage(1f);
        }
        if (collision.gameObject.CompareTag("Eye"))
        {
            EyeController eye = collision.gameObject.GetComponent<EyeController>();
            eye.TakeDamage(1f);
        }
    }
}
