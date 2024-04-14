using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerWeaponAim : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointPosition;
    private Transform aimMP40EndPointPosition;
    private Transform aimTommyGunEndPointPosition;
    private Transform aimShotGunEndPointPosition;
    private Transform gun;
    private Transform MP40;
    private Transform tommygun;
    private Transform shotgun;
    private SpriteRenderer gunSpriteRenderer;
    private PlayerController player;
    [SerializeField] private GameObject bulletPrefab;
    public bool canShoot = true;
    public bool fullAuto = false;
    private bool flipped = false;
    private float shootCoolDown;

    public static bool gunEquiped = false;
    public static bool MP40Equiped = false;
    public static bool tommyGunEquiped = false;
    public static bool shotgunEquiped = false;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerShot;
    private void Start()
    {
        PlayerWeaponAim.shotgunEquiped = false;
        PlayerWeaponAim.gunEquiped = true;
        PlayerWeaponAim.MP40Equiped = false;
        PlayerWeaponAim.tommyGunEquiped = false;
        player = FindObjectOfType<PlayerController>();
        aimTransform = FindObjectOfType<Aim>().transform;
        aimGunEndPointPosition = aimTransform.Find("GunEndPointPosition");
        aimMP40EndPointPosition = aimTransform.Find("MP40EndPointPosition");
        aimTommyGunEndPointPosition = aimTransform.Find("TommyGunEndPointPosition");
        aimShotGunEndPointPosition = aimTransform.Find("ShotGunEndPointPosition");
        gun = aimTransform.Find("Gun");
        shotgun = aimTransform.Find("Shotgun");
        MP40 = aimTransform.Find("MP40");
        tommygun = aimTransform.Find("TommyGun");
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
        if(ShopButtons.fullAutoBought)
        {
            shootCoolDown = 0.1f;
        }
        else
        {
            shootCoolDown = 0.2f;
        }

        if(PlayerWeaponAim.gunEquiped)
        {
            HandleAimingPistol();
            HandleShootingPistol();
        }
        else if(PlayerWeaponAim.shotgunEquiped)
        {

        }
        else if(PlayerWeaponAim.tommyGunEquiped)
        {

        }
        else if(PlayerWeaponAim.shotgunEquiped)
        {

        }
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

    private void HandleShootingShotGun()
    {

        if (ShopButtons.fullAutoBought)
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
