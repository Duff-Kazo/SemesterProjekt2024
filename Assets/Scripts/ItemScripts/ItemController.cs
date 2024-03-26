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
        if(player.monsterState < 3 )
        {
            player.monsterState++;
        }
    }

    public void ActivateShield()
    {
        PlayerController.shieldActivated = true;
        if (player.monsterState < 3)
        {
            player.monsterState++;
        }
    }

    public void ActivateOrbs()
    {
        PlayerController.orbsActivated = true;
        if (player.monsterState < 3)
        {
            player.monsterState++;
        }
    }

    public void ActivatePlague()
    {
        PlayerController.plagueActivated = true;
        if (player.monsterState < 3)
        {
            player.monsterState++;
        }
    }
}
