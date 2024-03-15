using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour
{
    private bool wasActivated;
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
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
                wasActivated = true;
                Destroy(gameObject);
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
