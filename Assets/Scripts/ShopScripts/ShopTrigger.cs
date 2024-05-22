using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    private AudioReverbFilter reverbFilter;

    private void Start()
    {
        shop.SetActive(false);
        reverbFilter = FindObjectOfType<AudioReverbFilter>();
        reverbFilter.enabled = true;
    }
    public void OpenShop()
    {
        reverbFilter.enabled = false;
        Interactable.gamePaused = true;
        Interactable.wasActivated = true;
        shop.SetActive(true);
    }

    public void CloseShop()
    {
        reverbFilter.enabled = true;
        Interactable.gamePaused = false;
        Interactable.wasActivated = false;
        shop.SetActive(false);
    }
}
