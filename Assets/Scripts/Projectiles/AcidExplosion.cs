using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distance = 2;
    [SerializeField] private float damage = 5;
    SoundManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<SoundManager>();
        gameManager.PlayExplosionSound();
        StartCoroutine(KillOnAnimationFinished());
        Collider2D hit = Physics2D.OverlapCircle(transform.position, distance, layerMask);
        if (hit != null)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                PlayerController player = hit.transform.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damage);
            }
        }
    }

    IEnumerator KillOnAnimationFinished()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
