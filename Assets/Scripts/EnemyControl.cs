using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameObject scoreUITextGO; //Reference to the text UI game object
    public GameObject ExplosionGO; //this is our explosion prefab 
    float speed; //for the enemy speed
    private bool hasHit = false; // Flag to check if the enemy has already been hit

    // Start is called before the first frame update
    void Start()
    {
        speed = 2f; //set speed

        //get the score text UI
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    // Update is called once per frame
    void Update()
    {
        //Get the enemy current position
        Vector2 position = transform.position;

        //Compute the enemy new position
        position = new Vector2(position.x, position.y-speed * Time.deltaTime);

        //Update the enemy position
        transform.position = position;

        //This is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        //if the enemy went outside the screen on the bottom, then destroy the enemy
        if (transform.position.y < min.y){
            Destroy(gameObject);

        }

    }

    

    void OnTriggerEnter2D(Collider2D col)
    {
        // Detect collision of the enemy with the player's ship or bullets
        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag01") || (col.tag == "PlayerBulletTag02") || (col.tag == "ShieldOnPlayerTag"))
        {
            // Only add points if the enemy hasn't been hit yet
            if (!hasHit)
            {
                PlayExplosion();

                // Add 100 points to the score
                scoreUITextGO.GetComponent<GameScore>().Score += 100;

                // Set the flag to true to prevent further score increases
                hasHit = true;
            }

            // Destroy the enemy ship after it is hit
            Destroy(gameObject);
        }
    }

    //function to instantiate an explosion
    void PlayExplosion(){
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);
        
        //set the position of the explosion
        explosion.transform.position = transform.position;
    }

}
