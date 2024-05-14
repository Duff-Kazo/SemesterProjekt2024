using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerController : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;
    public int healingAmount;
    public int magazineAmount;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        healingAmount = Random.Range(1, 3);
        magazineAmount = Random.Range(1, 5);
    }

    public void OpenLocker()
    {
        animator.SetBool("Opened", true);
        player.GetHealth(healingAmount);
        player.maxMagazines += magazineAmount;
        player.magazineBullets += player.maxBullets * magazineAmount;
        player.magazinesCount += magazineAmount;
    }
}
