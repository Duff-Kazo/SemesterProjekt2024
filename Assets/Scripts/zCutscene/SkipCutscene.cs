using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
    public UnityEvent skip;
    private float skipCount = 0;
    [SerializeField] private Image circle;
    [SerializeField] private GameObject skippingText;
    private void Start()
    {
        skippingText.SetActive(false);
    }

    private void Update()
    {
        circle.fillAmount = skipCount / 1;
        if(Input.GetKey(KeyCode.Space))
        {
            skipCount += 0.0025f;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            skipCount = 0;
        }

        if(skipCount >= 1)
        {
            circle.fillAmount = 1;
            skippingText.SetActive(true);
            skip.Invoke();
        }
    }
}
