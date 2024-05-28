using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ShopButtons : MonoBehaviour
{
    private PlayerController player;
    public static bool fullAutoBought = false;
    public static int soldOutCount = 0;
    public static float bulletDamage = 1;

    WarningPanel warningPanel;

    [SerializeField] private AudioSource click;

    [SerializeField] private int usesMoreMags;
    
    [SerializeField] private int usesMagUpgrade;
    [SerializeField] private int usesMaxHealth;
    [SerializeField] private int usesUpDamage;
    [SerializeField] private int usesLegMods;

    [SerializeField] private int usesShotgun;
    [SerializeField] private int usesTommyGun;
    [SerializeField] private int usesMP40;

    private Button approveButton;

    [SerializeField] private UnityEngine.UI.Button.ButtonClickedEvent shotGunClicked;
    [SerializeField] private UnityEngine.UI.Button.ButtonClickedEvent tommyGunClicked;
    [SerializeField] private UnityEngine.UI.Button.ButtonClickedEvent mp40Clicked;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        ShopButtons.soldOutCount = 0;
        usesMagUpgrade = 3;
        usesMaxHealth = 3;
        usesUpDamage = 2;
        usesLegMods = 1;
        usesShotgun = 1;
        usesTommyGun = 1;
        usesMP40 = 1;
        warningPanel = FindObjectOfType<WarningPanel>();
        approveButton = FindObjectOfType<YesButton>().gameObject.GetComponent<Button>();
        WarningPanel.numOfAssignations += 1;
        if(WarningPanel.numOfAssignations >= 4)
        {
            warningPanel.ClosePanel();
        }
    }

    public void MoreMags()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if ((usesMoreMags > 1))
        {
            usesText.text = usesMoreMags.ToString();
            if (player.bloodPoints >= 10)
            {
                usesMoreMags--;
                usesText.text = usesMoreMags.ToString();
                PlayClickSound();
                player.bloodPoints -= 10;
                Debug.Log("MoreMags");
                player.BuyItem("MoreMags", 10);
            }
            if (usesMoreMags <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
        if ((usesMP40 >= 1))
        {
            usesText.text = usesMP40.ToString();
            if (player.bloodPoints >= 200)
            {
                warningPanel.gameObject.SetActive(false);
                usesMP40--;
                usesText.text = usesMP40.ToString();
                PlayClickSound();
                player.bloodPoints -= 200;
                Debug.Log("FullAuto");
                player.BuyItem("FullAuto", 200);
            }
            if (usesMP40 <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
        if ((usesMagUpgrade >= 1))
        {
            usesText.text = usesMagUpgrade.ToString();
            if (player.bloodPoints >= 125)
            {
                usesMagUpgrade--;
                usesText.text = usesMagUpgrade.ToString();
                PlayClickSound();
                player.bloodPoints -= 125;
                Debug.Log("MagUpgrade");
                player.BuyItem("MagUpgrade", 125);
            }
            if (usesMagUpgrade <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
        if ((usesMaxHealth >= 1))
        {
            usesText.text = usesMaxHealth.ToString();
            if (player.bloodPoints >= 80)
            {
                usesMaxHealth--;
                usesText.text = usesMaxHealth.ToString();
                PlayClickSound();
                player.bloodPoints -= 80;
                player.BuyItem("Health", 80);
            }
            if (usesMaxHealth <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
        if ((usesUpDamage >= 1))
        {
            usesText.text = usesUpDamage.ToString();
            if (player.bloodPoints >= 110)
            {
                usesUpDamage--;
                usesText.text = usesUpDamage.ToString();
                PlayClickSound();
                player.bloodPoints -= 110;
                Debug.Log("UpDamage");
                player.BuyItem("UpDamage", 110);
            }
            if (usesUpDamage <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
        if ((usesLegMods >= 1))
        {
            usesText.text = usesLegMods.ToString();
            if (player.bloodPoints >= 100)
            {
                usesLegMods--;
                usesText.text = usesLegMods.ToString();
                PlayClickSound();
                player.bloodPoints -= 100;
                Debug.Log("LegMods");
                player.BuyItem("LegMods", 100);
            }
            if(usesLegMods <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }
    }

    public void WarningWeapon(string name)
    {   
        if (name == "Shotgun" && player.bloodPoints >= 300)
        {
            approveButton.onClick = shotGunClicked;
            warningPanel.gameObject.SetActive(true);
        }
        if (name == "Tommygun" && player.bloodPoints >= 400)
        {
            approveButton.onClick = tommyGunClicked;
            warningPanel.gameObject.SetActive(true);
        }
        if (name == "MP40" && player.bloodPoints >= 200)
        {
            approveButton.onClick = mp40Clicked;
            warningPanel.gameObject.SetActive(true);
        }
    }

    public void ShotGun()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if ((usesShotgun >= 1))
        {
            usesText.text = usesShotgun.ToString();
            if (player.bloodPoints >= 300)
            {
                warningPanel.gameObject.SetActive(false);
                usesShotgun--;
                usesText.text = usesShotgun.ToString();
                PlayClickSound();
                player.bloodPoints -= 300;
                Debug.Log("Shotgun");
                player.BuyItem("Shotgun", 300);
            }
            if (usesShotgun <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
            GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
            SoldOutbutton.SetActive(true);
            usesText.text = "0";
            ShopButtons.soldOutCount++;
        }
    }

    public void Tommygun()
    {
        Button button = GetComponentInParent<Button>();
        TextMeshProUGUI usesText = button.transform.Find("UsesText").GetComponent<TextMeshProUGUI>();
        if ((usesTommyGun >= 1))
        {
            usesText.text = usesShotgun.ToString();
            if (player.bloodPoints >= 400)
            {
                warningPanel.gameObject.SetActive(false);
                usesTommyGun--;
                usesText.text = usesShotgun.ToString();
                PlayClickSound();
                player.bloodPoints -= 400;
                Debug.Log("Tommygun");
                player.BuyItem("Tommygun", 400);
            }
            if (usesTommyGun <= 0)
            {
                button.interactable = false;
                GameObject description = button.transform.Find("Description").gameObject;
                description.SetActive(false);
                GameObject background = button.transform.Find("Background").gameObject;
                background.SetActive(false);
                GameObject SoldOutbutton = button.transform.Find("SoldOutText").gameObject;
                SoldOutbutton.SetActive(true);
                usesText.text = "0";
                ShopButtons.soldOutCount++;
            }
        }
        else
        {
            button.interactable = false;
            GameObject description = button.transform.Find("Description").gameObject;
            description.SetActive(false);
            GameObject background = button.transform.Find("Background").gameObject;
            background.SetActive(false);
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
