using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private GameObject spriteMask;
    [SerializeField] private GameObject parentGameObject;
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
        GameObject mask = Instantiate(spriteMask, transform.position, Quaternion.identity);
        mask.transform.parent = parentGameObject.transform;
        yield return new WaitForSeconds(0.2f);
        canSpawn = true;
    }
}

