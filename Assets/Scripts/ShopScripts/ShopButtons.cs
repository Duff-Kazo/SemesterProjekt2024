using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtons : MonoBehaviour
{
    private PlayerController player;
    public static bool fullAutoBought = false;
    public static int soldOutCount = 0;
    public static float bulletDamage = 1;
    [SerializeField] private AudioSource click;

    [SerializeField] private int usesMoreMags;
    [SerializeField] private int usesFullAuto;
    [SerializeField] private int usesMagUpgrade;
    [SerializeField] private int usesMaxHealth;
    [SerializeField] private int usesUpDamage;
    [SerializeField] private int usesLegMods;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        ShopButtons.soldOutCount = 0;
    }
    public void MoreMags()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesMoreMags > 1)
        {
            usesMoreMags--;
            usesText.text = usesMoreMags.ToString();
            if (player.bloodPoints >= 10)
            {
                PlayClickSound();
                player.bloodPoints -= 10;
                Debug.Log("MoreMags");
                player.BuyItem("MoreMags", 10);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }

    }
    public void FullAuto()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesFullAuto > 1)
        {
            usesFullAuto--;
            usesText.text = usesFullAuto.ToString();
            if (player.bloodPoints >= 200)
            {
                PlayClickSound();
                player.bloodPoints -= 200;
                Debug.Log("FullAuto");
                player.BuyItem("FullAuto", 200);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }
    }
    public void MagUpgrade()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesMagUpgrade > 1)
        {
            usesMagUpgrade--;
            usesText.text = usesMagUpgrade.ToString();
            if (player.bloodPoints >= 125)
            {
                PlayClickSound();
                player.bloodPoints -= 125;
                Debug.Log("MagUpgrade");
                player.BuyItem("MagUpgrade", 125);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }

    }
    public void MaxHealth()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesMaxHealth > 1)
        {
            usesMaxHealth--;
            usesText.text = usesMaxHealth.ToString();
            if (player.bloodPoints >= 80)
            {
                PlayClickSound();
                player.bloodPoints -= 80;
                player.BuyItem("Health", 80);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }

    }
    public void UpDamage()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesUpDamage > 1)
        {
            usesUpDamage--;
            usesText.text = usesUpDamage.ToString();
            if (player.bloodPoints >= 110)
            {
                PlayClickSound();
                player.bloodPoints -= 110;
                Debug.Log("UpDamage");
                player.BuyItem("UpDamage", 110);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }

    }
    public void LegMods()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if (usesLegMods > 1)
        {
            usesLegMods--;
            usesText.text = usesLegMods.ToString();
            if (player.bloodPoints >= 100)
            {
                PlayClickSound();
                player.bloodPoints -= 100;
                Debug.Log("LegMods");
                player.BuyItem("LegMods", 100);
            }
        }
        else
        {
            button.interactable = false;
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }
    }

    private void PlayClickSound()
    {
        click.Play();
    }
}
