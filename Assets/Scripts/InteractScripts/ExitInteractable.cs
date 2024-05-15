using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitInteractable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;
    private PlayerController player;

    private BossController boss;


    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        boss = FindObjectOfType<BossController>();
        Vector2 direction = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, layerMask);
        if (hit)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }

        if (isInRange && boss == null)
        {
            canvas.enabled = true;
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
        else
        {
            canvas.enabled = false;
        }

        canvas.transform.rotation = Quaternion.identity;
    }
}
