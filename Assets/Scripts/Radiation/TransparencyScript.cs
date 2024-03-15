using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransparencyScript : MonoBehaviour
{
    private float alphaStart = 0.0f;
    private float alphaEnd = 0.01f;
    private float duration = 0.75f;
    private Image thisImage;
    [SerializeField] private CanvasGroup radiationEffect;

    private void Start()
    {
        thisImage = transform.GetComponent<Image>();
    }

    private void Update()
    {
        float lerpSpeed = Mathf.PingPong(Time.time, duration) / duration;
        radiationEffect.alpha = Mathf.Lerp(alphaStart, alphaEnd, lerpSpeed);
    }
}
