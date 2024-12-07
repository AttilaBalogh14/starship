using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusHPControl : MonoBehaviour
{

    float speed;
    
    // Reference to the PlayerControl script
    public GameObject playerShip;  // Drag the player ship GameObject in the inspector

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;

        playerShip = GameObject.FindWithTag("PlayerShipTag");
    }

    // Update is called once per frame
    void Update()
    {
        //Get the object current position
        Vector2 position = transform.position;

        //Compute the object new position
        position = new Vector2(position.x, position.y-speed * Time.deltaTime);

        //Update the object position
        transform.position = position;

        //This is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        //if the object went outside the screen on the bottom, then destroy the enemy
        if (transform.position.y < min.y){
            Destroy(gameObject);

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Detect collision of the object with the player's ship
        if ((col.tag == "PlayerShipTag"))
        {
            // Increase the player's health by calling the method in the PlayerControl script
            PlayerControl playerControl = playerShip.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.IncreaseLives(1);  // Increase lives by 1
            }

            Destroy(gameObject);
        }
    }
}
