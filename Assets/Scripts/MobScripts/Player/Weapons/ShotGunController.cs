using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShotGunController : MonoBehaviour
{
    private Transform shotGunTransform;
    private Transform aimShotGunEndPointPosition;
    private Transform shotgun;
    private SpriteRenderer gunSpriteRenderer;
    private PlayerController player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCoolDown;
    public bool canShoot = true;
    public bool fullAuto = false;
    private bool flipped = false;
    

    [Header("ShotGun")]
    [SerializeField] private float spread;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerShot;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        shotGunTransform = FindObjectOfType<ShotGunAim>().transform;
        aimShotGunEndPointPosition = shotGunTransform.Find("ShotGunEndPointPosition");
        shotgun = shotGunTransform.Find("ShotGun");
        gunSpriteRenderer = shotgun.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Interactable.gamePaused)
        {
            return;
        }
        HandleAimingShotgun();
        if(canShoot)
        {
            HandleShootingShotGun();
        }
    }
    private void HandleAimingShotgun()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        shotGunTransform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (angle < 89 && angle > -89)
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


    private void ShootShotGun()
    {
        if (player.bulletCount >= 4)
        {
            player.bulletCount -= 4;
            playerShot.Play();
            for (int i = -2; i < 2; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, aimShotGunEndPointPosition.position, Quaternion.Euler(0, 0, i * spread));
                PlayerBulletScript bulletScript = bullet.GetComponent<PlayerBulletScript>();
                bulletScript.shotGunDamage = 0.5f;
                bullet.transform.up = shotGunTransform.up;
                bullet.transform.Rotate(new Vector3(0, 0, -90));
                bullet.transform.Rotate(new Vector3(0,0,i * spread));
            }
            CinemachineShake.instance.ShakeCamera(15, 0.1f);
        }
        StartCoroutine(ShootCoolDown());
    }

    private void HandleShootingShotGun()
    {

        if (Input.GetMouseButtonDown(0) && canShoot && !player.isReloading)
        {
            ShootShotGun();
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
