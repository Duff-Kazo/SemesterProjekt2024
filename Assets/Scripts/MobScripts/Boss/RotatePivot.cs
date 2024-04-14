using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePivot : MonoBehaviour
{
    private bool isRotating = false;
    private bool notInCoroutine = true;
    [SerializeField] private float rotateSpeed;
    private int rotateDirection = 1;

    private void FixedUpdate()
    {
        isRotating = true;
        transform.Rotate(new Vector3(0, 0, rotateDirection) * rotateSpeed);
        if (notInCoroutine)
        {
            StartCoroutine(RotateCoolDown());
        }
    }

    private IEnumerator RotateCoolDown()
    {
        notInCoroutine = false;
        yield return new WaitForSeconds(Random.Range(3, 5));
        rotateDirection *= -1;
        notInCoroutine = true;
    }
}
