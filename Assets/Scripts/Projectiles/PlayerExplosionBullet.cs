using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosionBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionTimer = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0, 0, 1), -90);
        StartCoroutine(ExplosionTimer());
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(explosionTimer);
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HeadMouth") || collision.gameObject.CompareTag("Crawler") || collision.gameObject.CompareTag("Eye") || collision.gameObject.CompareTag("Boss"))
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            Explode();
        }
    }


    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
