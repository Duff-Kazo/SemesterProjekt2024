using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EaseFloatingHealthBar : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Slider healthBarSlider;
    private float lerpSpeed = 0.05f;
    private float scaleX;
    private void Start()
    {
        scaleX = transform.localScale.x;
    }
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = target.position + offset;

        if (healthBarSlider.value != slider.value)
        {
            slider.value = Mathf.Lerp(slider.value, healthBarSlider.value, lerpSpeed);
        }
    }
}
