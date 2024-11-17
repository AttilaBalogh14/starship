using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO; //reference to our game manager

    public GameObject PlayerBulletGO01; //This is our players bullet 01 prefab
    public GameObject PlayerBulletGO02; //This is our players bullet 02 prefab
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject ExplosionGO; //this is our explosion prefab

    //Reference to the lives ui text
    public Text LivesUIText;

    const int MaxLives = 3; //Maximum player lives
    int Lives; //Current player lives

    public float speed;
    private PlusHPSpawn plusHPSpawnScript; // A PlusHPSpawn script referenciája
    public GameObject ShieldOnPlayer; // Reference to the shield object
    public void Init()
    {
        Lives = MaxLives;

        //Update the lives ui text
        LivesUIText.text = Lives.ToString();

        //reset the players postion to the center of the screen
        transform.position = new Vector2(0,0); 

        //set this player game object to active
        gameObject.SetActive(true);
    }

    // Method to increase player's lives
    public void IncreaseLives(int amount)
    {
        Lives += amount;
        // Update the lives UI
        LivesUIText.text = Lives.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the PlusHPSpawn script from the GameManager
        plusHPSpawnScript = GameManagerGO.GetComponent<GameManager>().PlusHPSpawnGO.GetComponent<PlusHPSpawn>();
    }

    // Update is called once per frame
    void Update()
    {

        //fire bullets when the spacebar is pressed
        if(Input.GetKeyDown("space")){

            //Play the laser sound effect
            GetComponent<AudioSource>().Play();

            //instantiate the first bullet
            GameObject bullet01 = (GameObject)Instantiate (PlayerBulletGO01);
            bullet01.transform.position = bulletPosition01.transform.position; //set the bullet initial position
            
            //instantiate the seconf bullet
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO02);
            bullet02.transform.position = bulletPosition02.transform.position; //set the bullet initial position

        }

        float x = Input.GetAxisRaw("Horizontal"); //the value will be -1, 0 or 1 (for left, no input, and right)
        float y = Input.GetAxisRaw("Vertical"); //the value will be -1, 0 or 1 (for down, no input, and up)

        //now based on the input we conpute a direction vector, and we normalize it to get a unit vector
        Vector2 direction = new Vector2 (x, y).normalized;

        //now we call the function that computes and sets the players position
        Move(direction);


    }

    void Move(Vector2 direction){
        //find the screen limits to the players movement (left, right, top and bottom edges of the screen)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0)); // this is the bottom-left point corner of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1)); // this is the top-right point corner of the screen

        max.x = max.x - 0.225f; //subtract the player sprite half width
        min.x = min.x + 0.225f; //add the player sprite half width

        max.y = max.y - 0.285f; //subtract the player sprite half height
        min.y = min.y + 0.285f; //add the player sprite half height

        //Get the players current position

        Vector2 pos = transform.position;

        //Calculate the new position

        pos += direction * speed * Time.deltaTime;

        // Make sure the new position is not outside the screen

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        //Update the players position

        transform.position = pos;
    }

   void OnTriggerEnter2D(Collider2D col)
{
    // Ha a pajzs aktív, akkor eltávolítjuk az ellenséges objektumot, de nem vonunk le életet
    if (ShieldOnPlayer.activeSelf)
    {
        if (col.tag == "EnemyBulletTag" || col.tag == "EnemyShipTag" || col.tag == "AsteroidTag")
        {
            Destroy(col.gameObject); // Töröljük az ütköző objektumot (pl. ellenséges golyó, aszteroida)
            return; // Ne folytassuk tovább a kódot, ha a pajzs aktív, mivel nem kell életet vonni
        }
    }

    // Ha a pajzs nem aktív, akkor le kell vonni életet, ha az ütközés ellenséggel vagy golyóval történt
    if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag") || (col.tag == "AsteroidTag"))
    {
        PlayExplosion(); // Játékos robbanása

        Lives--; // Egy élet levonása
        LivesUIText.text = Lives.ToString(); // Életek UI frissítése

        if (Lives == 0) // Ha a játékos meghalt
        {
            // Változtassuk meg a játék állapotát game over-re
            GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);

            // A játékos hajóját eltüntetjük
            gameObject.SetActive(false);
        }

        // Ha 1 élet maradt, értesítjük a PlusHPSpawn scriptet
        if (Lives == 1 && plusHPSpawnScript != null)
        {
            plusHPSpawnScript.CheckAndSpawnPlusHP(Lives);
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
