using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateButtons : MonoBehaviour
{
    [SerializeField] private Transform slot1;
    [SerializeField] private Transform slot2;
    [SerializeField] private Transform slot3;
    [SerializeField] private Transform slot4;

    [SerializeField] private List<GameObject> shopButtons;

    [SerializeField] private GameObject moreMagsButton;

    public Transform descriptionPosition;

    List<GameObject> buttons = new List<GameObject>();

    //private void Start()
    //{
    //    GenerateShop();
    //}

    public void GenerateShop()
    {
        foreach (var button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
        List<GameObject> validButtons = new List<GameObject>();
        validButtons.AddRange(shopButtons);
        if (ShopButtons.fullAutoBought)
        {
            validButtons.RemoveAt(0);
        }
        int index1 = Random.Range(0, validButtons.Count);
        GameObject firstItem = Instantiate(validButtons[index1], slot1.position, Quaternion.identity);
        firstItem.transform.SetParent(slot1.transform, true);
        buttons.Add(firstItem);
        validButtons.RemoveAt(index1);
        int index2 = Random.Range(0, validButtons.Count);
        GameObject secondItem = Instantiate(validButtons[index2], slot2.position, Quaternion.identity);
        secondItem.transform.SetParent(slot2.transform, true);
        buttons.Add(secondItem);
        validButtons.RemoveAt(index2);
        int index3 = Random.Range(0, validButtons.Count);
        GameObject thirdItem = Instantiate(validButtons[index3], slot3.position, Quaternion.identity);
        thirdItem.transform.SetParent(slot3.transform, true);
        buttons.Add(thirdItem);
        validButtons.RemoveAt(index3);
        GameObject moreMagsItem = Instantiate(moreMagsButton, slot4.position, Quaternion.identity);
        moreMagsItem.transform.SetParent(slot4.transform, true);

        if (firstItem.name == "Shotgun" || firstItem.name == "Tommygun" || firstItem.name == "FullAuto")
        {
            validButtons.Remove(firstItem);
        }
        if (secondItem.name == "Shotgun" || secondItem.name == "Tommygun" || secondItem.name == "FullAuto")
        {
            validButtons.Remove(secondItem);
        }
        if (thirdItem.name == "Shotgun" || thirdItem.name == "Tommygun" || thirdItem.name == "FullAuto")
        {
            validButtons.Remove(thirdItem);
        }
    }
}
