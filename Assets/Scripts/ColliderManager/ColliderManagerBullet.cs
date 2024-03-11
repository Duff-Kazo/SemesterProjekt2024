using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManagerBullet : MonoBehaviour
{
    private HeadMouthBullet headMouthBullet;
    [SerializeField] private float offset = -3;
    void Start()
    {
        headMouthBullet = GetComponentInParent<HeadMouthBullet>();
    }

    void Update()
    {
        transform.position = headMouthBullet.transform.position - new Vector3(0, offset);
    }
}
