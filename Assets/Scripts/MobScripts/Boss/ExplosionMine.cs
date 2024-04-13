using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExplosionMine : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Light2D light;
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionTimer = 3f;
    private float explosionCount = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0, 0, 1), -90);
    }

    private void Update()
    {
        if(explosionCount <= 1)
        {
            light.color = Color.green;
        }
        else if(explosionCount <= 2 && explosionCount >= 1)
        {
            light.color = Color.yellow;
        }
        else if(explosionCount >= 2)
        {
            light.color = Color.red;
        }
        if(explosionCount < explosionTimer)
        {
            explosionCount += Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
