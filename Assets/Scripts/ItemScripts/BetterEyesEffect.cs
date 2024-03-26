using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterEyesEffect : MonoBehaviour
{
    [SerializeField] private GameObject enemyGlobalLight;

    void Update()
    {
        if(PlayerController.eyesActivated)
        {
            enemyGlobalLight.SetActive(true);
        }
        else
        {
            enemyGlobalLight.SetActive(false);
        }
    }
}
