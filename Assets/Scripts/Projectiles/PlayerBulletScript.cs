using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float playerBulletDamage = 1;
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
            headMouth.TakeDamage(playerBulletDamage);
            Destroy(gameObject);
        }
    }
}
