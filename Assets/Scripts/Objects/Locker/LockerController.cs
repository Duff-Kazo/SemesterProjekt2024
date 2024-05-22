using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockerController : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;
    public int healingAmount;
    public int magazineAmount;
    [SerializeField] private AudioSource lockerOpen;
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Animator bulletAnimator;
    [SerializeField] private Animator healthAnimator;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        healingAmount = Random.Range(1, 3);
        magazineAmount = Random.Range(1, 3);
        bulletText.text = "+ " + (magazineAmount * 16) + " bullets";
        healthText.text = "+ " + healingAmount + " health";
    }

    public void OpenLocker()
    {
        bulletAnimator.SetTrigger("TriggerText");
        healthAnimator.SetTrigger("TriggerText");
        lockerOpen.Play();
        animator.SetBool("Opened", true);
        player.GetHealth(healingAmount);
        player.maxMagazines += magazineAmount;
        player.magazineBullets += player.maxBullets * magazineAmount;
        player.magazinesCount += magazineAmount;
    }
}
