using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP40Controll : MonoBehaviour
{
    private Transform MP40Transform;
    private Transform aimMP40EndPointPosition;
    private Transform MP40;
    private SpriteRenderer MP40SpriteRenderer;
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
        MP40Transform = FindObjectOfType<MP40Aim>().transform;
        MP40 = MP40Transform.transform.Find("MP40");
        aimMP40EndPointPosition = MP40Transform.Find("MP40EndPointPosition");
        MP40SpriteRenderer = MP40.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Interactable.gamePaused)
        {
            return;
        }
        HandleAimingMP40();
        if (canShoot)
        {
            HandleShootingMP40();
        }
    }
    private void HandleAimingMP40()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        MP40Transform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (angle < 89 && angle > -89)
        {
            MP40SpriteRenderer.flipY = false;
            if (!flipped)
            {
                flipped = true;
            }
        }
        else
        {
            MP40SpriteRenderer.flipY = true;
            if (flipped)
            {
                flipped = false;
            }
        }
    }


    private void ShootMP40()
    {
        if (player.bulletCount > 0)
        {
            playerShot.Play();
            player.bulletCount -= 1;
            GameObject bullet = Instantiate(bulletPrefab, aimMP40EndPointPosition.position, Quaternion.identity);
            PlayerBulletScript bulletScript = bullet.GetComponent<PlayerBulletScript>();
            bulletScript.applyTommyGunDamage = true;
            bullet.transform.up = MP40Transform.up;
            bullet.transform.Rotate(new Vector3(0, 0, -90));
        }
        StartCoroutine(ShootCoolDown());
    }

    private void HandleShootingMP40()
    {
        if (Input.GetMouseButton(0) && canShoot && !player.isReloading)
        {
            ShootMP40();
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
