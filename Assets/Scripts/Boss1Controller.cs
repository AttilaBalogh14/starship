using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    public float speed = 0f;                   // Főgonosz sebessége
    public float shootingInterval = 2f;       // Lövések közötti időköz
    public int Lives = 12;                    // Boss1 élete
    public GameObject bulletPrefab;           // Lövedék prefabja
    public Transform bulletSpawnPoint;        // Lövedék kilövési pozíciója
    public Transform player;                  // Játékos pozíciója
    GameObject scoreUITextGO;                 // Pontszám UI
    private bool movingRight = true;          // Kezdeti mozgásirány jobbra
    public GameObject ExplosionGO;            //explosion prefab

    void Start()
    {
        // Kezdetben véletlenszerűen állítja be az irányt
        movingRight = Random.value > 0.5f;

        // Megkeressük a pontszámkezelő UI-t 
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    void Update()
    {
        Move();
        // A lövés logika kiegészíthető itt
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
            
            PlayExplosion();

            // Növeljük a játékos pontszámát
            if (scoreUITextGO != null)
            {
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
            }

            // Ha az élet 0, elpusztítjuk a boss1-et
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

}

