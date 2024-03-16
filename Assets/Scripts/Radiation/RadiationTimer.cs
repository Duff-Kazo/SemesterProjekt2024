using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class RadiationTimer : MonoBehaviour
{
    public float remainingTime = 300;
    private PlayerController player;
    [SerializeField] private TextMeshProUGUI radiationText;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        radiationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if(remainingTime <= 1)
        {
            player.Die();
        }
    }
}
