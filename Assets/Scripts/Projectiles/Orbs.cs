using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();   
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
        transform.Rotate(new Vector3(0, 0, 8f));
    }
}
