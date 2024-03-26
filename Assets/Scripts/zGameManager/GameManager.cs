using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioSource enemyDeathSound;
    [SerializeField] private AudioSource teleport;

    private Orbs orbs;

    private void Start()
    {
        orbs = FindObjectOfType<Orbs>();
    }
    private void Update()
    {
        orbs.transform.gameObject.SetActive(PlayerController.orbsActivated);
    }
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
