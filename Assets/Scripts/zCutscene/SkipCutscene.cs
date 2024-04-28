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
    [SerializeField] private float timeToSkip;
    private void Start()
    {
        skippingText.SetActive(false);
    }

    private void Update()
    {
        circle.fillAmount = skipCount / timeToSkip;
        if(Input.GetKey(KeyCode.Space))
        {
            skipCount += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            skipCount = 0;
        }

        if(skipCount >= timeToSkip)
        {
            circle.fillAmount = 1;
            skippingText.SetActive(true);
            skip.Invoke();
        }
    }
}
