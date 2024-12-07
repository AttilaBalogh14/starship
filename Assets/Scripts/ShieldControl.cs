using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldControl : MonoBehaviour
{
    float speed;
    
    // Reference to the PlayerControl script
    public GameObject playerShip;  // Drag the player ship GameObject in the inspector
    public GameObject ShieldOnPlayer; // Reference to the player's shield
    private float shieldduration = 15f;
    private float shieldtimer;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;

        playerShip = GameObject.FindWithTag("PlayerShipTag");
        //shieldtimer = shieldduration;
        ShieldOnPlayer = GameObject.FindWithTag("ShieldOnPlayerTag");
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
        
        
        // If the shield is active, start counting down the timer
        if (ShieldOnPlayer.activeSelf && ShieldOnPlayer != null) 
        {
            shieldtimer -= Time.deltaTime;
            if (shieldtimer <= 0f)
            {
                DeactivateShield(); // Deactivate shield after the duration
            }
        }
    }

     void OnTriggerEnter2D(Collider2D col)
    {
        // Detect collision of the object with the player's ship
        if ((col.tag == "PlayerShipTag"))
        {
            GameManager.Instance.ActivateShield();  // Call the ActivateShield function

            Destroy(gameObject);
        }
    }
    
    // Deactivates the shield after the specified duration
    void DeactivateShield()
    {
        if (ShieldOnPlayer != null)
        {
            ShieldOnPlayer.SetActive(false);  // Deactivate the shield
            shieldtimer = shieldduration; // Reset the shield timer
        }
    }
}
