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
    [SerializeField] private TextMeshProUGUI dialogue;

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
            dialogue.text = "I dont sell shit to monsters";
        }
        else if(player.monsterState == 2)
        {
            dialogue.text = "Are you okay man? Doesn't look like it";
        }
        else if (player.monsterState == 1)
        {
            dialogue.text = "Be careful out there";
        }
        else if (player.monsterState == 0)
        {
            dialogue.text = "Take your pick";
        }

        if(player.monsterState != 3 && ShopButtons.soldOutCount == 4)
        {
            dialogue.text = "We're sold out man. Happy hunting";
        }

        if(!Interactable.gamePaused)
        {
            DissableDescription();
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
