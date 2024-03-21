using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMusic : MonoBehaviour
{
    PlayerController player;
    [SerializeField] AudioSource shopMusic;
    [SerializeField] AudioSource shopMusicDamped;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        shopMusic.volume = 0;
        shopMusicDamped.volume = 0;
    }

    void Update()
    {
        Vector2 vecToPlayer = transform.position - player.transform.position;

        if(vecToPlayer.magnitude < 30 && !Interactable.inShop)
        {
            shopMusic.volume = 0;
            shopMusicDamped.volume = 2f / vecToPlayer.magnitude;
        }
        else if(vecToPlayer.magnitude < 10 && Interactable.inShop)
        {
            shopMusicDamped.volume = 0;
            shopMusic.volume = 1;
        }
        else
        {
            shopMusicDamped.volume = 0;
            shopMusic.volume = 0;
        }
    }
}
