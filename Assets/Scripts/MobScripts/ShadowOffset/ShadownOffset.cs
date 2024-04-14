using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadownOffset : MonoBehaviour
{
    [SerializeField] private float yOffset;
    private Transform parent;

    private void Start()
    {
        parent = GetComponentInParent<Transform>();
    }

    void Update()
    {
        transform.position = parent.position + new Vector3(0, yOffset, 0);
    }
}
