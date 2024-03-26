using UnityEngine;
using System.Collections;

public class Radiation : MonoBehaviour
{
    [SerializeField] private GameObject radiationEffect;
    public static bool radiationTimerActive = false;
    private RadiationTimer radiationTimer;
    // Use this for initialization
    void Start()
    {
        radiationEffect.SetActive(false);
        radiationTimer = FindObjectOfType<RadiationTimer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            radiationEffect.SetActive(true);
            Radiation.radiationTimerActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(radiationTimer != null)
            {
                radiationEffect.SetActive(false);
                Radiation.radiationTimerActive = false;
            }
        }
    }
}
