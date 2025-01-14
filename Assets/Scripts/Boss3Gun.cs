using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // A lövedék prefab
    public Transform firePointLeft;  // A bal oldali lövési pont
    public Transform firePointRight; // A jobb oldali lövési pont
    public Transform firePointLeft2;  // A bal oldali lövési pont
    public Transform firePointRight2; // A jobb oldali lövési pont
    public Transform firePointCenter; // A kozepso lövési pont
    float fireRate = 1.5f;    // Lövési sebesség (másodpercben)
    float nextFireTime = 0.85f; // Következő lövés ideje

    private Transform player; // A játékos pozíciója

    void Start()
    {
        // Keressük meg a játékost a "Player" tag alapján
        GameObject playerShip = GameObject.Find("PlayerGO");
        if (playerShip != null)
        {
            player = playerShip.transform;
        }
    }

    void Update()
    {
        // Ha elérkezett az idő a lövéshez
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Következő lövés időzítése
        }
    }

    void Shoot()
    {
        if (player == null) return; // Ha nincs játékos, ne lőjünk

        // Számítsuk ki az irányt a játékos felé
        Vector2 directionToPlayer = (player.position - firePointLeft.position).normalized;

        // Lövedék kilövése bal oldalról
        GameObject leftBullet = Instantiate(bulletPrefab, firePointLeft.position, Quaternion.identity);
        leftBullet.GetComponent<Boss3Bullet>().SetDirection(directionToPlayer);

        // Lövedék kilövése jobb oldalról
        GameObject rightBullet = Instantiate(bulletPrefab, firePointRight.position, Quaternion.identity);
        rightBullet.GetComponent<Boss3Bullet>().SetDirection(directionToPlayer);

        GameObject leftBullet2 = Instantiate(bulletPrefab, firePointLeft2.position, Quaternion.identity);
        leftBullet2.GetComponent<Boss3Bullet>().SetDirection(directionToPlayer);

        // Lövedék kilövése jobb oldalról
        GameObject rightBullet2 = Instantiate(bulletPrefab, firePointRight2.position, Quaternion.identity);
        rightBullet2.GetComponent<Boss3Bullet>().SetDirection(directionToPlayer);

        // Lövedék kilövése kozeprol
        GameObject centerBullet = Instantiate(bulletPrefab, firePointCenter.position, Quaternion.identity);
        centerBullet.GetComponent<Boss3Bullet>().SetDirection(directionToPlayer);
    }
}