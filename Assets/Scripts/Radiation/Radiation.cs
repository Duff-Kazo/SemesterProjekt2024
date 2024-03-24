using UnityEngine;
using System.Collections;

public class Radiation : MonoBehaviour
{
    [SerializeField] private GameObject radiationEffect;
    private RadiationTimer radiationTimer;
    // Use this for initialization
    void Start()
    {
        radiationEffect.SetActive(false);
        radiationTimer = FindObjectOfType<RadiationTimer>();
        radiationTimer.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            radiationEffect.SetActive(true);
            radiationTimer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(radiationTimer != null)
            {
                radiationEffect.SetActive(false);
                radiationTimer.enabled = false;
            }
        }
    }
}
