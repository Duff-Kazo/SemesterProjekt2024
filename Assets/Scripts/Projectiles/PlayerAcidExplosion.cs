using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAcidExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distance = 2;
    [SerializeField] private float damage = 4;
    private List<HeadMouthController> headMouthList = new List<HeadMouthController>();
    private List<CrawlerController> crawlerList = new List<CrawlerController>();
    private List<EyeController> eyeList = new List<EyeController>();
    SoundManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<SoundManager>();
        gameManager.PlayExplosionSound();
        StartCoroutine(KillOnAnimationFinished());
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, distance, layerMask);
        if (hit != null)
        {
            for(int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.gameObject.CompareTag("HeadMouth"))
                {
                    HeadMouthController headMouth = hit[i].transform.gameObject.GetComponent<HeadMouthController>();
                    headMouth.TakeDamage(damage);
                }
                if (hit[i].transform.gameObject.CompareTag("Crawler"))
                {
                    CrawlerController crawler = hit[i].transform.gameObject.GetComponent<CrawlerController>();
                    crawler.TakeDamage(damage);
                }
                if (hit[i].transform.gameObject.CompareTag("Eye"))
                {
                    EyeController eye = hit[i].transform.gameObject.GetComponent<EyeController>();
                    eye.TakeDamage(damage);
                }
                if (hit[i].transform.gameObject.CompareTag("Boss"))
                {
                    BossController boss = hit[i].transform.gameObject.GetComponent<BossController>();
                    boss.TakeDamage(damage);
                }
                if (hit[i].transform.gameObject.CompareTag("ShopKeeper"))
                {
                    ShopKeeperController shopKeeper = hit[i].transform.gameObject.GetComponent<ShopKeeperController>();
                    shopKeeper.TakeDamage(damage);
                }
            }
            
        }
    }

    IEnumerator KillOnAnimationFinished()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
