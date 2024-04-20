using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private BoxCollider2D bossWall;
    [SerializeField] private Transform bossSpawnPoint;

    private GameObject bossInstance;

    private SpriteRenderer bossWallGraphic;
    private bool wasActivated = false;

    private void Start()
    {
        bossWallGraphic = GetComponentInParent<SpriteRenderer>();
        bossWallGraphic.enabled = false;
        bossWall.enabled = false;
        wasActivated = false;
    }

    private void Update()
    {
        if(bossInstance == null)
        {
            bossWallGraphic.enabled = false;
            bossWall.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!wasActivated)
            {
                wasActivated = true;
                bossInstance = Instantiate(boss, bossSpawnPoint.position, Quaternion.identity);
                bossWall.enabled = true;
                bossWallGraphic.enabled = true;
            }
        }
    }
}
