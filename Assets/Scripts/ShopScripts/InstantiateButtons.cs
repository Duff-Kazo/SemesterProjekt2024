using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        if(ShopButtons.fullAutoBought)
        {
            shopButtons.RemoveAt(0);
        }
        int index1 = Mathf.RoundToInt(Random.Range(0, shopButtons.Count));
        GameObject firstItem = Instantiate(shopButtons[index1], slot1.position, Quaternion.identity);
        firstItem.transform.SetParent(slot1.transform, true);
        shopButtons.RemoveAt(index1);
        int index2 = Mathf.RoundToInt(Random.Range(0, shopButtons.Count));
        GameObject secondItem = Instantiate(shopButtons[index2], slot2.position, Quaternion.identity);
        secondItem.transform.SetParent(slot2.transform, true);
        shopButtons.RemoveAt(index2);
        int index3 = Mathf.RoundToInt(Random.Range(0, shopButtons.Count));
        GameObject thirdItem = Instantiate(shopButtons[index3], slot3.position, Quaternion.identity);
        thirdItem.transform.SetParent(slot3.transform, true);
        shopButtons.RemoveAt(index3);
        GameObject moreMagsItem = Instantiate(moreMagsButton, slot4.position, Quaternion.identity);
        moreMagsItem.transform.SetParent(slot4.transform, true);
    }
}
