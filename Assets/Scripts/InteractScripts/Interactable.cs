using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public static bool wasActivated;
    public bool isInRange;
    public KeyCode interactKey;
    public KeyCode interactKey1;
    public UnityEvent interactAction;
    public UnityEvent interactAction1;
    public static bool inShop = false;
    [SerializeField] private Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        wasActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && !wasActivated)
        {
            canvas.enabled = true;
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
                inShop = true;
                wasActivated = true;
            }
        }
        else if(isInRange && wasActivated)
        {
            canvas.enabled = false;
            if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(interactKey1))
            {
                interactAction1.Invoke();
                inShop = false;
                wasActivated = false;
            }
        }
        else
        {
            canvas.enabled = false;
        }

        canvas.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = true;
            Debug.Log("Player is in range");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
        Debug.Log("Player is out of range");
    }
}
