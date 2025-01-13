using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    private float speed = 10f;                   // Főgonosz sebessége
    float Lives, MaxLives = 330;                    // Boss3 élete 330
    public Transform player;                  // Játékos pozíciója
    GameObject scoreUITextGO;                 // Pontszám UI
    private bool movingRight = true;          // Kezdeti mozgásirány jobbra
    private bool isPaused = false;             // Megállt-e a boss jelenleg
    private float pauseTimer = 0f;          // Szünet számláló
    private float nextDashTime;
    public GameObject ExplosionGO;            //explosion prefab
    Vector3 originalPosition;                 // Eredeti pozíció mentése

    public HealthBar healthBar;

    void Start()
    {
        // Kezdetben véletlenszerűen állítja be az irányt
        movingRight = Random.value > 0.5f;

        // Megkeressük a pontszámkezelő UI-t 
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        SetNextDashTime();

        Lives = MaxLives;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(Lives, MaxLives);
        }
    }

    void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = false; // Szünet vége
            }
        }
        else
        {
            Move();
            if (Random.Range(0f, 1f) < 0.001f) // ~% esély frame-enként
            {
                isPaused = true;
                pauseTimer = Random.Range(0.2f, 0.5f); // Véletlen szünet 1-2 másodperc
            }

            // Véletlenszerű gyors mozgás előre és vissza
            if (Time.time>=nextDashTime) // ~% esély frame-enként
            {
                DashToBottomAndBack();
                SetNextDashTime();
            }
        }
    }


    // Főgonosz jobbra-balra mozgatása
    void Move()
    {
        // Jobbra vagy balra mozdul el az aktuális irány alapján
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        // Ha eléri a képernyő széleit, irányt vált
        if (transform.position.x >= 4.5f) // Jobb széle (változtatható a képernyő szélesség szerint)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -4.5f) // Bal széle (változtatható)
        {
            movingRight = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Érzékeli, ha a játékos lövedékei vagy hajója eltalálja a boss1-et
        if (col.CompareTag("PlayerBulletTag01") || col.CompareTag("PlayerBulletTag02") || col.CompareTag("PlayerShipTag"))
        {
            // Csökkentjük a boss életét
            Lives--;

            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(Lives, MaxLives);
            }
            
            PlayExplosion();

            // Növeljük a játékos pontszámát
            if (scoreUITextGO != null)
            {
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
            }

            // Ha az élet 0, elpusztítjuk a boss3-at
            if (Lives <= 0)
            {
                Destroy(gameObject);
            }

            // Elpusztítjuk a játékos lövedékét (ha az találta el)
            if (col.CompareTag("PlayerBulletTag01") || col.CompareTag("PlayerBulletTag02"))
            {
                Destroy(col.gameObject);
            }
        }
    }
     //function to intantiate an explosion
    void PlayExplosion(){
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        //set the position of the explosion
        explosion.transform.position = transform.position;
    }

    // Gyors mozgás előre és vissza
    void DashToBottomAndBack()
    {
        //originalPosition = transform.position; // Eredeti pozíció mentése

        // Előre a képernyő aljára
        transform.position = new Vector2(transform.position.y, -1f);

        // Kis várakozás, majd vissza az eredeti pozícióra
        Invoke(nameof(ReturnToOriginalPosition), 1f); // 1 másodperc várakozás
    }

    void ReturnToOriginalPosition()
    {
        // Visszatérés az eredeti pozícióba
        //transform.position = originalPosition;
        transform.position = new Vector2(transform.position.x, 1.9f);

    }

    void SetNextDashTime()
{
    // Véletlenszerűen generálunk egy következő időpontot 5-10 másodpercen belül
    nextDashTime = Time.time + Random.Range(5f, 10f);

}

}






