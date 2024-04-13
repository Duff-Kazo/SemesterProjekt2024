using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(0.59342f*1.6f, 0.59342f*1.6f, 0);
    }
}
