using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    public static int numOfAssignations = 0;
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
