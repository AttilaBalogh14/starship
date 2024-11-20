using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShootControl : MonoBehaviour
{

    float speed;
    public PlayerControl playerControl; // Hivatkozás a PlayerControl scriptre

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;

        // Ha még nincs hozzárendelve a playerControl, próbáld meg megtalálni a PlayerControl komponenst a szülő GameObjecten
        if (playerControl == null)
        {
            playerControl = FindObjectOfType<PlayerControl>(); // Megkeressük a PlayerControl komponenst a játékban
        }
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
        // Ha a játékos hajója ütközik a PowerShoot objektummal
        if (col.tag == "PlayerShipTag")
        {
            // Aktiváljuk a PowerShoot-ot a játékos számára
            playerControl.ActivatePowerShoot();

            // Töröljük a PowerShoot objektumot
            Destroy(gameObject);
        }
    }
}
