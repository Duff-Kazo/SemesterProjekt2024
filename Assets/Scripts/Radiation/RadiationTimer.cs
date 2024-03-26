using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadiationTimer : MonoBehaviour
{
    private float remainingTime = 300;
    private float baseRemainingTime = 300;
    private PlayerController player;
    private bool wasActivated = false;
    [SerializeField] private TextMeshProUGUI radiationText;
    void Start()
    {
        remainingTime = baseRemainingTime;
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        radiationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (!wasActivated && PlayerController.shieldActivated)
        {
            wasActivated = true;
            baseRemainingTime *= 2;
            remainingTime = baseRemainingTime;
        }
        if (Radiation.radiationTimerActive)
        {
            remainingTime -= Time.deltaTime;


            if (remainingTime <= 1)
            {
                player.Die();
            }
        }
    }
}
