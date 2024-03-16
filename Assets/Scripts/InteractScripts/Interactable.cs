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
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        wasActivated = false;
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, layerMask);
        if(hit)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
        
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
}
