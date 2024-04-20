using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterLevelSystem : MonoBehaviour
{
    private PlayerController player;
    private PlayerAcidBall monsterWeapon;
    [Header("UI")]
    [SerializeField] private GameObject abilityUnlocked;

    private bool ability1 = false;
    private bool ability2 = false;
    private bool ability3 = false;
    private bool ability4 = false;
    void Start()
    {
        player = GetComponent<PlayerController>();
        monsterWeapon = GetComponent<PlayerAcidBall>();
        abilityUnlocked.SetActive(false);
    }

    void Update()
    {
        if (player.level >= 5)
        {
            player.dashEnabled = true;
            if (!ability1)
            {
                ability1 = true;
                StartCoroutine(LevelUpAnimation());
            }
            
        }
        if (player.level >= 10)
        {
            player.explosionBulletEnabled = true;
            if (!ability2)
            {
                ability2 = true;
                StartCoroutine(LevelUpAnimation());
            }
        }
        if (player.level >= 15)
        {
            player.shieldEnabled = true;
            if (!ability3)
            {
                ability3 = true;
                StartCoroutine(LevelUpAnimation());
            }
        }
        if (player.level >= 20)
        {
            player.AcidFullAutoEnabled = true;
            if (!ability4)
            {
                ability4 = true;
                StartCoroutine(LevelUpAnimation());
            }
        }
    }

    private IEnumerator LevelUpAnimation()
    {
        abilityUnlocked.SetActive(true);
        yield return new WaitForSeconds(2f);
        abilityUnlocked.SetActive(false);
    }
}
