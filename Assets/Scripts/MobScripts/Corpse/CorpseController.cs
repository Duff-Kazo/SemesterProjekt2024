using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject bloodParticles;
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
        Instantiate(bloodParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
