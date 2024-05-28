using UnityEngine;
using System.Collections;

public class Radiation : MonoBehaviour
{
    [SerializeField] private GameObject radiationEffect;
    [SerializeField] private GameObject radiationText;
    [SerializeField] private AudioSource radiationSound;
    public static bool radiationTimerActive = false;
    private RadiationTimer radiationTimer;
    // Use this for initialization
    void Start()
    {
        radiationEffect.SetActive(false);
        radiationText.SetActive(false);
        radiationTimer = FindObjectOfType<RadiationTimer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            radiationEffect.SetActive(true);
            radiationText.SetActive(true);
            radiationSound.Play();
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
                radiationText.SetActive(false);
                radiationSound.Stop();
                Radiation.radiationTimerActive = false;
            }
        }
    }
}
