using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private PlayerController player;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        CreateItemButton("Bigger Magazines", 175, 0);
        CreateItemButton("FullAuto", 210, 1);
        CreateItemButton("More Magazines", 30, 2);
        CreateItemButton("Poison Bullets", 125, 3);
        CreateItemButton("Health", 10, 4);
        HideShop();
    }

    private void CreateItemButton(string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        float shopHeight = 1.5f;
        shopItemRectTransform.anchorMin = new Vector3(0, -shopHeight * positionIndex);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //Clicked on shop item button
            TryBuyItem(itemName, itemCost);
        };
    }

    private void TryBuyItem(string itemName, int itemCost)
    {
        if(player.bloodPoints >= itemCost)
        {
            player.bloodPoints -= itemCost;
        }
    }

    public void HideShop()
    {
        shopItemTemplate.gameObject.SetActive(false);
    }

    public void ShowShop()
    {
        shopItemTemplate.gameObject.SetActive(true);
    }
}
