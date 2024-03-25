using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioSource enemyDeathSound;
    [SerializeField] private AudioSource teleport;
    public void PlayExplosionSound()
    {
        explosionSound.Play();
    }
    public void PlayEnemyDeathSound()
    {
        enemyDeathSound.Play();
    }

    public void PlayBossTeleport()
    {
        teleport.Play();
    }
}
