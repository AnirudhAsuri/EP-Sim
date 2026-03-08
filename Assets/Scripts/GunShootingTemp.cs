using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShootingTemp : MonoBehaviour
{
    public InputActionReference gunShootAction;

    [SerializeField] private Transform bulletSpawningPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        Debug.Log("Enabled");
        gunShootAction.action.Enable();
        gunShootAction.action.started += GunFire;
    }

    private void OnDisable()
    {
        gunShootAction.action.started -= GunFire;
        gunShootAction.action.Disable();
    }

    private void GunFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Fire");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawningPoint.position, bulletSpawningPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawningPoint.forward * bulletSpeed;
    }

    /*public void FireGun()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawningPoint.position, bulletSpawningPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawningPoint.forward * bulletSpeed;
    }*/
}