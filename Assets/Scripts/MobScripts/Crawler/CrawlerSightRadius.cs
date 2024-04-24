using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerSightRadius : MonoBehaviour
{
    private CrawlerController crawler;
    [SerializeField] private Transform headPosition;

    private void Start()
    {
        crawler = GetComponentInParent<CrawlerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            crawler.isAggro = true;
        }
    }
}
