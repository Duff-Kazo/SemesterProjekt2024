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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            shop.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(shop != null)
        {
            shop.SetActive(false);
        }
    }
}
