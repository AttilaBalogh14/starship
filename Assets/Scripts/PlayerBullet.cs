using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the bullets current position
        Vector2 position = transform.position;

        //compute the bullets new position
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        //update the bullets position
        transform.position = position;

        //This is the top-right point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2 (1,1));

        //if the bullet went outside the screen on the top, then destroy the bullet 
        if (transform.position.y > max.y){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        //detect collision of the player bullet with an enemy ship
        if ((col.tag == "EnemyShipTag") || (col.tag == "Boss1ShipTag")|| (col.tag == "Boss2ShipTag")|| (col.tag == "Boss3ShipTag")){
            //destroy this player bullet
            Destroy(gameObject);
        }
    }

}
