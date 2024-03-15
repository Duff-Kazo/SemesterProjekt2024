using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private PlayerController player;
    private GameObject enemyLight;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemyLight = transform.Find("EnemyLight").gameObject;
        enemyLight.SetActive(false);
    }

    

    public void ActivateEyes()
    {
        enemyLight.SetActive(true);
    }

    public void ActivateShield()
    {
        player.ActivateMonsterItems("Shield");
    }

    public void ActivateOrbs()
    {
        player.ActivateMonsterItems("Orbs");
    }

    public void ActivatePlague()
    {
        player.ActivateMonsterItems("Plague");
    }
}
