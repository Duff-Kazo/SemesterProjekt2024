using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGunAim : MonoBehaviour
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
