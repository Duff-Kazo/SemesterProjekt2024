using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaveDescription : MonoBehaviour
{
    private Image background;
    private TextMeshProUGUI description;
    [SerializeField] private AudioSource hoverSound;
    private PlayerController player;
    [SerializeField] private GameObject dialogue;

    private void Start()
    {
        background = transform.Find("Background").GetComponent<Image>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        description.enabled = false;
        background.enabled = false;
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(player.monsterState == 3)
        {
            dialogue.SetActive(true);
        }
        else
        {
            dialogue.SetActive(false);
        }
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
