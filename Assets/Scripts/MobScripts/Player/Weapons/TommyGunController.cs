using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGunController : MonoBehaviour
{
    private Transform tommyGunTransform;
    private Transform aimTommyGunEndPointPosition;
    private Transform tommyGun;
    private SpriteRenderer tommyGunSpriteRenderer;
    private PlayerController player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCoolDown;
    public bool canShoot = true;
    public bool fullAuto = false;
    private bool flipped = false;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerShot;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        tommyGunTransform = FindObjectOfType<TommyGunAim>().transform;
        aimTommyGunEndPointPosition = tommyGunTransform.Find("TommyGunEndPointPosition");
        tommyGun = tommyGunTransform.Find("TommyGun");
        tommyGunSpriteRenderer = tommyGun.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Interactable.gamePaused)
        {
            return;
        }
        HandleAimingTommyGun();
        if (canShoot)
        {
            HandleShootingTommyGun();
        }
    }
    private void HandleAimingTommyGun()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        tommyGunTransform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (angle < 89 && angle > -89)
        {
            tommyGunSpriteRenderer.flipY = false;
            if (!flipped)
            {
                flipped = true;
            }
        }
        else
        {
            tommyGunSpriteRenderer.flipY = true;
            if (flipped)
            {
                flipped = false;
            }
        }
    }


    private void ShootTommyGun()
    {
        if (player.bulletCount > 0)
        {
            player.bulletCount -= 1;
            playerShot.Play();
            GameObject bullet = Instantiate(bulletPrefab, aimTommyGunEndPointPosition.position, Quaternion.identity);
            PlayerBulletScript bulletScript = bullet.GetComponent<PlayerBulletScript>();
            bulletScript.applyTommyGunDamage = true;
            bullet.transform.up = tommyGunTransform.up;
            bullet.transform.Rotate(new Vector3(0, 0, -90));
        }
        StartCoroutine(ShootCoolDown());
    }

    private void HandleShootingTommyGun()
    {
        if (Input.GetMouseButton(0) && canShoot && !player.isReloading)
        {
            ShootTommyGun();
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
