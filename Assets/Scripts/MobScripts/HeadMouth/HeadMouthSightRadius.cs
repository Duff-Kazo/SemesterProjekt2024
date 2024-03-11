using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMouthSightRadius : MonoBehaviour
{
    private HeadMouthController headmouth;
    private PlayerController player;
    private CircleCollider2D range;
    private bool playerInSight = false;
    [SerializeField] private Transform headPosition;

    private void Start()
    {
        headmouth = GetComponentInParent<HeadMouthController>();
        player = FindObjectOfType<PlayerController>();
        range = transform.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            headmouth.isAggro = true;
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    headmouth.isAggro = false;
    //}
}
