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
    [SerializeField] private LayerMask layerMask;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        canvas.enabled = false;
        wasActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 1.5f, layerMask);
        if(hit)
        {
            if(hit.transform.gameObject.CompareTag("Player"))
            {
                isInRange = true;
            }
            else
            {
                isInRange = false;
            }
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
