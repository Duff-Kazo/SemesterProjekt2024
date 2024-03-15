using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransparencyScript : MonoBehaviour
{
    private float alphaStart = 0.0f;
    private float alphaEnd = 1.0f;
    private float duration = 1.0f;

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        GetComponent<Image>().material.color = new Color(
            GetComponent<Image>().material.color.r,
            GetComponent<Image>().material.color.g,
            GetComponent<Image>().material.color.b,
            Mathf.Lerp(alphaStart, 0.01f, lerp)
        );
    }
}
