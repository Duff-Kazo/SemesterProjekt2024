using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI description;
    [SerializeField] private AudioSource hoverSound;

    private void Start()
    {
        background = transform.Find("Background").GetComponent<Image>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();

        description.enabled = false;
        background.enabled = false;
    }

    public void EnableDescription()
    {
        hoverSound.Play();
        description.enabled = true;
        background.enabled = true;
    }

    public void DissableDescription()
    {
        description.enabled = false;
        background.enabled = false;
    }
}
