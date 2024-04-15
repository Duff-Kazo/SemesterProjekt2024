using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private TextMeshProUGUI corpseText;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(CorpseCoolDown());
    }

    private void Update()
    {
        if (player.monsterState == 3)
        {
            corpseText.text = "F - Eat Corpse";
        }
        else
        {
            corpseText.text = "F - Inject Corpse Blood";
        }
    }

    public void EatCorpse()
    {
        Destroy(gameObject);
        if (player.health < player.maxHealth)
        {
            player.health++;
        }
        Instantiate(bloodParticles, transform.position, Quaternion.identity);
    }

    private IEnumerator CorpseCoolDown()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }
    
}
