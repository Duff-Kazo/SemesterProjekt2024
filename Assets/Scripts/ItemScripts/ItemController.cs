using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemController : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject enemyLight;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
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
