using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSightRadius : MonoBehaviour
{
    private EyeController eye;
    private CircleCollider2D range;
    [SerializeField] private Transform headPosition;

    private void Start()
    {
        eye = GetComponentInParent<EyeController>();
        range = transform.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eye.isAggro = true;
            Debug.Log("Player Detected");
        }
    }
}
