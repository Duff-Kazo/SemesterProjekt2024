using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMouthSightRadius : MonoBehaviour
{
    private HeadMouthController headmouth;
    private PlayerController player;
    private CircleCollider2D range;
    [SerializeField] private Transform headPosition;

    private void Start()
    {
        headmouth = GetComponentInParent<HeadMouthController>();
        if(headmouth == null)
        {
            throw new Exception("HeadMouthSightRadius: headmouth == null");
        }
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            throw new Exception("HeadMouthSightRadius: player == null");
        }
        range = transform.GetComponent<CircleCollider2D>();
        if (range == null)
        {
            throw new Exception("HeadMouthSightRadius: range == null");
        }
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
