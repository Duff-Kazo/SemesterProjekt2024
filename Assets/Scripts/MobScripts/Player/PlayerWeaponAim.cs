using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;

public class PlayerWeaponAim : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointPosition;
    private Transform gun;
    private SpriteRenderer gunSpriteRenderer;
    private PlayerController player;
    [SerializeField] private GameObject bulletPrefab;
    public bool canShoot = true;
    public bool fullAuto = false;
    private bool flipped = false;
    [SerializeField] private float shootCoolDown;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerShot;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        aimTransform = FindObjectOfType<Aim>().transform;
        aimGunEndPointPosition = aimTransform.Find("GunEndPointPosition");
        gun = aimTransform.Find("Gun");
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
    }
    private void HandleAimingPistol()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if(angle < 89 && angle > -89)
        {
            gunSpriteRenderer.flipY = false;
            if (!flipped)
            {
                flipped = true;
            }
        }
        else
        {
            gunSpriteRenderer.flipY = true;
            if (flipped)
            {
                flipped = false;
            }
        }
    }
    private void Update()
    {
        if(Interactable.gamePaused)
        {
            return;
        }
        HandleAimingPistol();
        HandleShootingPistol();
    }

    private void HandleShootingPistol()
    {
        if(ShopButtons.fullAutoBought)
        {
            if (Input.GetMouseButton(0) && canShoot && !player.isReloading)
            {
                ShootPistol();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && canShoot && !player.isReloading)
            {
                ShootPistol();
            }
        }
        
    }
    private void ShootPistol()
    {
        if (player.bulletCount > 0)
        {
            player.bulletCount -= 1;
            playerShot.Play();
            GameObject bullet = Instantiate(bulletPrefab, aimGunEndPointPosition.position, Quaternion.identity);
            bullet.transform.up = aimTransform.up;
            bullet.transform.Rotate(new Vector3(0, 0, -90));
            CinemachineShake.instance.ShakeCamera(2, 0.1f);
            StartCoroutine(ShootCoolDown());
        }
    }
    private IEnumerator ShootCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
    }


    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
