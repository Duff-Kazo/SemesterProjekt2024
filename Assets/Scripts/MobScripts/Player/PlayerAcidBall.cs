using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAcidBall : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private float shootCoolDown = 0.1f;
    private bool canShoot = true;
    void Update()
    {
        if(!Interactable.inShop)
        {
            HandleShooting();
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        if(canShoot)
        {
            shootSound.Play();
            Vector3 difference = GetMouseWorldPosition() - bulletOrigin.position;
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            bullet.transform.right = difference;
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
