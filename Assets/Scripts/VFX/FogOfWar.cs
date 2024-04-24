using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private GameObject spriteMask;
    private bool canSpawn = true;

    void Update()
    {
        if(canSpawn && !Interactable.gamePaused)
        {
            StartCoroutine(SpawnSpriteMask());
        }
    }

    private IEnumerator SpawnSpriteMask()
    {
        canSpawn = false;
        Instantiate(spriteMask, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        canSpawn = true;
    }
}

