using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipCutscene : MonoBehaviour
{
    public UnityEvent skip;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            skip.Invoke();
        }
    }
}
