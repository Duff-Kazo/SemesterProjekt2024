using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioSource enemyDeathSound;
    [SerializeField] private AudioSource teleport;
    [SerializeField] private AudioSource eatBody;

    private Orbs orbs;

    private void Start()
    {
        orbs = FindObjectOfType<Orbs>();
    }
    private void Update()
    {
        if(orbs != null)
        {
            orbs.transform.gameObject.SetActive(PlayerController.orbsActivated);
        }
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

    public void EatBody()
    {
        eatBody.Play();
    }
}
