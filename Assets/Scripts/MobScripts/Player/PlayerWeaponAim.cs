using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerWeaponAim : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointPosition;
    private Transform gun;
    private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private GameObject bulletPrefab;

    private bool canShoot = true;
    private void Start()
    {
        aimTransform = FindObjectOfType<Aim>().transform;
        aimGunEndPointPosition = aimTransform.Find("GunEndPointPosition");
        gun = aimTransform.Find("Gun");
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
    }
    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if(angle < 89 && angle > -89)
        {
            gunSpriteRenderer.flipY = false;
        }
        else
        {
            gunSpriteRenderer.flipY = true;
        }
    }
    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if(Input.GetMouseButtonDown(0) && canShoot)
        {
            GameObject bullet = Instantiate(bulletPrefab, aimGunEndPointPosition.position, Quaternion.identity);
            bullet.transform.up = aimTransform.up;
            bullet.transform.Rotate(new Vector3(0, 0, -90));
            StartCoroutine(ShootCoolDown());
        }
    }

    private IEnumerator ShootCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.1f);
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
