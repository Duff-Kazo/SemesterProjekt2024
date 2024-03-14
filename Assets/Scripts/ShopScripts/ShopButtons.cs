using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    private PlayerController player;
    public static bool fullAutoBought = false;
    public static float bulletDamage = 1;
    [SerializeField] private GameObject fullAutoButton;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void MoreMags()
    {
        if(player.bloodPoints >= 30)
        {
            player.bloodPoints -= 30;
            Debug.Log("MoreMags");
            player.BuyItem("MoreMags", 30);
        }
    }
    public void FullAuto()
    {
        if(!fullAutoBought)
        {
            if (player.bloodPoints >= 200)
            {
                player.bloodPoints -= 200;
                Debug.Log("FullAuto");
                player.BuyItem("FullAuto", 200);
                fullAutoButton.SetActive(false);
            }
        }
    }
    public void MagUpgrade()
    {
        if (player.bloodPoints >= 125)
        {
            player.bloodPoints -= 125;
            Debug.Log("MagUpgrade");
            player.BuyItem("MagUpgrade", 125);
        }
    }
    public void MaxHealth()
    {
        if (player.bloodPoints >= 80)
        {
            player.bloodPoints -= 80;
            player.BuyItem("Health", 80);
        }
    }
    public void UpDamage()
    {
        if (player.bloodPoints >= 110)
        {
            player.bloodPoints -= 110;
            Debug.Log("UpDamage");
            player.BuyItem("UpDamage", 110);
        }
    }
    public void LegMods()
    {
        if (player.bloodPoints >= 100)
        {
            player.bloodPoints -= 100;
            Debug.Log("LegMods");
            player.BuyItem("LegMods", 100);
        }
    }
}
