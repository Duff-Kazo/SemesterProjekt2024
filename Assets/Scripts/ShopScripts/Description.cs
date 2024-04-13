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
    private Button button;
    private PlayerController player;
    InstantiateButtons buttonController;

    private void Start()
    {
        background = transform.Find("Background").GetComponent<Image>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        button = transform.GetComponent<Button>();
        description.enabled = false;
        background.enabled = false;
        player = FindObjectOfType<PlayerController>();
        buttonController = FindObjectOfType<InstantiateButtons>();
    }

    private void Update()
    {
        if(player.monsterState == 3)
        {
            button.interactable = false;
            description.text = "Fuck off";
        }
        description.transform.position = buttonController.descriptionPosition.position;
        background.transform.position = buttonController.descriptionPosition.position;
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
