using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMouthBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject acidCollisionPrefab;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0,0,1), -90);
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(bulletDamage);
            Instantiate(acidCollisionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Walls"))
        {
            Instantiate(acidCollisionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
