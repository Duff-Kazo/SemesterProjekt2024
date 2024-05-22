using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    private PlayerController player;
    private SoundManager gameManager;
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private TextMeshProUGUI corpseText;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<SoundManager>();
        StartCoroutine(CorpseCoolDown());
    }

    private void Update()
    {
        if (player.monsterState == 3)
        {
            corpseText.text = "F - Eat Corpse";
        }
    }

    public void EatCorpse()
    {
        if(player.monsterState == 3)
        {
            Destroy(gameObject);
            if (player.health < player.maxHealth)
            {
                player.health++;
            }
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            gameManager.EatBody();
        }
    }

    private IEnumerator CorpseCoolDown()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }
    
}
