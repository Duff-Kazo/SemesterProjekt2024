using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerSightRadius : MonoBehaviour
{
    private CrawlerController crawler;
    private PlayerController player;
    private CircleCollider2D range;
    private bool playerInSight = false;
    [SerializeField] private Transform headPosition;

    private void Start()
    {
        crawler = GetComponentInParent<CrawlerController>();
        player = FindObjectOfType<PlayerController>();
        range = transform.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            crawler.isAggro = true;
        }
    }
}
