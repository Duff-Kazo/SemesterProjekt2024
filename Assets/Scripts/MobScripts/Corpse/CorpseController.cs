using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void EatCorpse()
    {
        if(player.health < player.maxHealth)
        {
            player.health += 1;
        }
        Destroy(gameObject);
    }
    
}
