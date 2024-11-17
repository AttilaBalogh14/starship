using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidControl : MonoBehaviour
{
    float speed;  // Sebesség, amivel az aszteroida mozog
    private Vector2 direction;  // A mozgás iránya
    //public AsteroidSpawner asteroidSpawner; // A spawner referencia, ahonnan a side értéket kapjuk
    void Start()
    {

        speed = 3f;

        //int side = asteroidSpawner.choseSide();

        SetDirectionBasedOnSide();

        /*// Véletlenszerű irány generálása - véletlen szög [0, 360] fok között
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // Sebesség normalizálása, hogy az irány mindig ugyanakkora sebességgel történjen
        direction.Normalize();
        */
    }

    void Update()
    {
        // Az aszteroida pozíciójának frissítése a sebesség és irány alapján
        transform.Translate(direction * speed * Time.deltaTime);


        //int side = asteroidSpawner.choseSide();    

        destroyAsteroid();
        
    }

    public void SetDirectionBasedOnSide()
    {

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

        //Get the asteroid current position
        //Vector2 position = transform.position;
        
        //get a reference to the players ship
        GameObject playerShip = GameObject.Find("PlayerGO");

        if (playerShip != null) { //if the player is not dead

            //compute the asteroid direction toward the players ship
            direction = playerShip.transform.position - transform.position;
        }

        /*if (transform.position.y == max.y) {
            // Ha a felső oldalon jelent meg, akkor lefelé mozogjon
            float angle = Random.Range(90f, 270f);
            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        } else if (transform.position.x == min.x) {
            // Ha a bal oldalon jelent meg, akkor jobbra
            float angle = Random.Range(0f, 180f);
            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        } else if (transform.position.x == max.x) {
            // Ha a jobb oldalon jelent meg, akkor balra
            float angle = Random.Range(180f, 360f);
            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }
        */
    }
        
        public void destroyAsteroid(){

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

        //Get the asteroid current position
        Vector2 position = transform.position;

        //Compute the asteroid new position
        /*position = new Vector2(position.x, position.y-speed * Time.deltaTime);

        //Update the asteroid position
        transform.position = position;
        */

        if (transform.position.y < min.y){
            Destroy(gameObject);
        } else if (transform.position.x > max.x) {
            Destroy(gameObject);
        } else if (transform.position.x < min.x) {
            Destroy(gameObject);
        }

        // Sebesség normalizálása
        direction.Normalize();
    }

    void OnTriggerEnter2D (Collider2D col){
        //detect collision of the player ship with an asteroid
        if ((col.tag == "PlayerShipTag") || (col.tag == "ShieldOnPlayerTag")) {
            Destroy(gameObject);
        }
    }
}
