
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;  // A lövedék sebessége
    private Vector2 _direction;  // A lövedék iránya
    private bool isReady = false; // A lövedék készen áll-e a mozgásra

    void Awake()
    {
        // Beállítjuk az alapértelmezett sebességet és irányt
        speed = 5f;
        isReady = false;
    }

    // A lövedék irányának beállítása
    public void SetDirection(Vector2 direction)
    {
        // Az irány normalizálása (egységvektor)
        _direction = direction.normalized;

        isReady = true; // Beállítjuk a lövedéket, hogy kész a mozgásra
    }

    void Update()
    {
        if (isReady)
        {
            // A lövedék új pozíciójának kiszámítása
            Vector2 position = transform.position;

            // A lövedék új pozíciója a sebesség és az irány alapján
            position += _direction * speed * Time.deltaTime;

            // A lövedék új pozíciójának frissítése
            transform.position = position;

            // A lövedék eltűnik, ha elhagyja a képernyőt
            CheckIfOutOfBounds();
        }
    }

    // Ha a lövedék kimegy a képernyőről, akkor eltüntetjük
    private void CheckIfOutOfBounds()
    {
        // A képernyő bal alsó és jobb felső sarkának koordinátái
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Ha a lövedék a képernyőn kívül van, töröljük
        if (transform.position.x < min.x || transform.position.x > max.x || transform.position.y < min.y || transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    // Ha a lövedék ütközik a játékos hajójával
    void OnTriggerEnter2D(Collider2D col)
    {
        // Ha a lövedék a játékos hajójával ütközik
        if (col.CompareTag("PlayerShipTag"))
        {
            // A játékos hajóját érintette, itt elhelyezhetjük a sebzés logikát is
            Destroy(gameObject);  // A lövedék eltűnik

        }
    }
}