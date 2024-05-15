using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemController : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void ActivateEyes()
    {
        PlayerController.eyesActivated = true;
        player.ChangeMonsterState();
    }

    public void ActivateShield()
    {
        PlayerController.shieldActivated = true;
        player.ChangeMonsterState();
    }

    public void ActivateOrbs()
    {
        PlayerController.orbsActivated = true;
        player.ChangeMonsterState();
    }

    public void ActivatePlague()
    {
        PlayerController.plagueActivated = true;
        player.ChangeMonsterState();
    }
}
