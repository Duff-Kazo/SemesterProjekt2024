using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpriteMaskPerformanceHandler : MonoBehaviour
{
    private GameObject playerMask;
    private void Start()
    {
        if(playerMask == null)
        {
            playerMask = GameObject.FindWithTag("FogSpriteMask");
        }
    }
    void Update()
    {
        if(transform.position == playerMask.transform.position)
        {
            Destroy(gameObject);
        }
    }
}