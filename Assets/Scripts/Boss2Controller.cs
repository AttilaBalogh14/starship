using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    private float speed = 6f;                   // Főgonosz sebessége
    float Lives, MaxLives = 5;                    // Boss2 élete 200
    public Transform player;                  // Játékos pozíciója
    GameObject scoreUITextGO;                 // Pontszám UI
    private bool movingRight = true;          // Kezdeti mozgásirány jobbra
    private bool isPaused = false; // Megállt-e a boss jelenleg
    private float pauseTimer = 0f; // Szünet számláló
    public GameObject ExplosionGO;            //explosion prefab
    public delegate void Boss2DestroyedEvent();
    public static event Boss2DestroyedEvent OnBoss2Destroyed;

    public HealthBar healthBar;

    private void Awake()
    {
        // Automatikus keresés, ha nincs explicit hozzárendelve
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }
    }

    void Start()
    {
        // Kezdetben véletlenszerűen állítja be az irányt
        movingRight = Random.value > 0.5f;

        // Megkeressük a pontszámkezelő UI-t 
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

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
            if (Random.Range(0f, 1f) < 0.01f) // ~% esély frame-enként
            {
                isPaused = true;
                pauseTimer = Random.Range(0.2f, 0.5f); // Véletlen szünet 1-2 másodperc
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
        // Érzékeli, ha a játékos lövedékei vagy hajója eltalálja a boss2-et
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

            // Ha az élet 0, elpusztítjuk a boss2-et
            if (Lives == 0)
            {
                OnBoss2Destroyed?.Invoke(); // Esemény indítása
                Destroy(gameObject);
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
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

}
