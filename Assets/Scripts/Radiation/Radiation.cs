using UnityEngine;
using System.Collections;

public class Radiation : MonoBehaviour
{
    [SerializeField] private GameObject radiationEffect;
    // Use this for initialization
    void Start()
    {
        radiationEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            radiationEffect.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            radiationEffect.SetActive(false);
        }
    }
}
