using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shop;

    private void Start()
    {
        shop.SetActive(false);
    }
    public void OpenShop()
    {
        Interactable.inShop = true;
        Interactable.wasActivated = true;
        shop.SetActive(true);
    }

    public void CloseShop()
    {
        Interactable.inShop = false;
        Interactable.wasActivated = false;
        shop.SetActive(false);
    }
}
