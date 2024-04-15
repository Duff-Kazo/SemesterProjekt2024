using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP40Aim : MonoBehaviour
{
    PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}
