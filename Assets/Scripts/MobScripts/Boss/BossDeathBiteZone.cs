using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathBiteZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(7);
        }
    }
}
