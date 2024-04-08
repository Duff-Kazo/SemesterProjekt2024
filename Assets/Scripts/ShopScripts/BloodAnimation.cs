using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAnimation : MonoBehaviour
{
    [SerializeField] private Transform brainPoint;
    void Update()
    {
        transform.position = brainPoint.position - new Vector3(0,100,0);
    }
}
