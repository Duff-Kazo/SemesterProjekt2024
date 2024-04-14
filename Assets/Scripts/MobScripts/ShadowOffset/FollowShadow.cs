using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShadow : MonoBehaviour
{
    private Transform shadow;
    [SerializeField] private float yOffset;
    void Start()
    {
        shadow = transform.Find("Shadow");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = shadow.position + new Vector3(0, yOffset, 0);
    }
}
