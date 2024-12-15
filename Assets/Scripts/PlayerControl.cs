using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO; //reference to our game manager
    public GameManager GameManagerSrcipt;

    public GameObject PlayerBulletGO01; //This is our players bullet 01 prefab
    public GameObject PlayerBulletGO02; //This is our players bullet 02 prefab
    public GameObject PlayerBulletGO03; //This is our players bullet 03 prefab
    public GameObject PlayerBulletGO04; //This is our players bullet 04 prefab
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject bulletPosition03;
    public GameObject bulletPosition04;
    public GameObject extra_part01;
    public GameObject extra_part02;
    public GameObject ExplosionGO; //this is our explosion prefab

    //Reference to the lives ui text
    public Text LivesUIText;
    private bool hasPowerShoot = false; // A PowerShoot aktiválása
    public float powerShootActiveTime = 15f; // PowerShoot aktív időtartama
    private float powerShootTimer = 0f; // Időzítő, ami számolja az időt
    const int MaxLives = 3; //Maximum player lives
    int Lives; //Current player lives

    public float speed;
    private PlusHPSpawn plusHPSpawnScript; // A PlusHPSpawn script referenciája
    public GameObject ShieldOnPlayer; // Reference to the shield object
    private bool isFlashing = false; // Állapot, hogy a hajó villog-e
    private float flashTimer = 0f; // Időzítő a villogás vezérléséhez
    public float flashDuration = 2f; // Az idő, amely alatt a hajó "villog"
    public SpriteRenderer playerSpriteRenderer; // A játékos hajójának SpriteRenderer-e
    public SpriteRenderer Extra_Part01SpriteRenderer; // A játékos hajójának SpriteRenderer-e
    public SpriteRenderer Extra_Part02SpriteRenderer; // A játékos hajójának SpriteRenderer-e

    //private int flashCycleCount = 0; // Nyomon követjük, hány ciklus ment végbe
    private float flashInterval = 0.15f; // Az intervallum, amelyenként váltogatunk a láthatóságot
    private float flashCooldown = 0f; // A villogás ütemezése, hogy minden 0.5 másodpercben történjen
    private bool isVisible = true; // Az állapot, hogy a hajó jelenleg látható-e vagy sem
    public AudioSource shootAudio;
    public AudioSource getdemageAudio;
    private bool isInvulnerable = false;
    private float invulnerabilityTime = 2f; // 1 másodperc invulnerabilitás
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
        GameManagerSrcipt = GameManagerGO.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If PowerShoot is active, count down the timer
        if (hasPowerShoot)
        {
            powerShootTimer -= Time.deltaTime; // Decrease the timer each frame

            if (powerShootTimer <= 0f) // If the timer reaches 0, deactivate PowerShoot
            {
                DeactivatePowerShoot();
            }
        }

        // Ha a hajó villog, kezeljük a villogás logikáját
    if (isFlashing)
    {
        flashTimer -= Time.deltaTime; // Csökkentjük az időt a villogás hosszához

        // Ha a villogás ideje még nem telt el
        if (flashTimer > 0f)
        {
            flashCooldown -= Time.deltaTime; // Csökkentjük a villogás ütemezését

            // Minden 0.5 másodpercben váltsuk a láthatóságot
            if (flashCooldown <= 0f)
            {
                isVisible = !isVisible; // Váltunk a láthatóság állapotán
                playerSpriteRenderer.enabled = isVisible; // Frissítjük a láthatóságot
                if(hasPowerShoot){
                    Extra_Part01SpriteRenderer.enabled = isVisible;
                    Extra_Part02SpriteRenderer.enabled = isVisible;
                }

                flashCooldown = flashInterval; // Reseteljük az időzítőt a következő villanásra
            }
        }
        else
        {
            // Ha a villogás ideje letelt, fejezzük be
            isFlashing = false;
            playerSpriteRenderer.enabled = true; // Biztosítjuk, hogy végleg látható legyen
        }
    }
        //fire bullets when the spacebar is pressed
        if(Input.GetKeyDown("space")){

            //Play the laser sound effect
            //GetComponent<AudioSource>().Play();
            shootAudio.Play();

            //instantiate the first bullet
            GameObject bullet01 = (GameObject)Instantiate (PlayerBulletGO01);
            bullet01.transform.position = bulletPosition01.transform.position; //set the bullet initial position
            
            //instantiate the seconf bullet
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO02);
            bullet02.transform.position = bulletPosition02.transform.position; //set the bullet initial position

             // Instantiate the third bullet (only if PowerShoot is active)
            if (hasPowerShoot)
            {
                GameObject bullet03 = Instantiate(PlayerBulletGO03);
                bullet03.transform.position = bulletPosition03.transform.position;
            
                GameObject bullet04 = Instantiate(PlayerBulletGO04);
                bullet04.transform.position = bulletPosition04.transform.position;
                
            }

        }

        float x = Input.GetAxisRaw("Horizontal"); //the value will be -1, 0 or 1 (for left, no input, and right)
        float y = Input.GetAxisRaw("Vertical"); //the value will be -1, 0 or 1 (for down, no input, and up)

        //now based on the input we conpute a direction vector, and we normalize it to get a unit vector
        Vector2 direction = new Vector2 (x, y).normalized;

        //now we call the function that computes and sets the players position
        Move(direction);


    }

    public void Move(Vector2 direction){
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

    void StartInvulnerability() {
        isInvulnerable = true;
        Invoke("EndInvulnerability", invulnerabilityTime);
    }

    void EndInvulnerability() {
        isInvulnerable = false;
    }

   void OnTriggerEnter2D(Collider2D col)
{
    // Ha a pajzs aktív, akkor eltávolítjuk az ellenséges objektumot, de nem vonunk le életet
    if (GameManagerSrcipt.isShieldActive == true)
    {
        if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag") || (col.tag == "AsteroidTag") 
        || (col.tag == "Boss1BulletTag") || (col.tag == "Boss2BulletTag") || (col.tag == "Boss3BulletTag"))
        {
            // Ha pajzs van, töröljük az ütköző objektumot
            Destroy(col.gameObject); // Töröljük az ütköző objektumot (pl. ellenséges golyó, aszteroida)
            return; // Ne folytassuk tovább, ha a pajzs aktív
        }
    }

    // Ha a pajzs nem aktív, akkor le kell vonni életet, ha az ütközés ellenséggel vagy golyóval történt
    if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag") || (col.tag == "AsteroidTag") 
    || (col.tag == "Boss1ShipTag") || (col.tag == "Boss1BulletTag") || (col.tag == "Boss2ShipTag") 
    || (col.tag == "Boss2BulletTag") || (col.tag == "Boss3ShipTag") || (col.tag == "Boss3BulletTag"))
    {
       {
            if (isInvulnerable) return; // Ignore collisions if invulnerable

            if (GameManagerSrcipt.isShieldActive == false) // Ha a pajzs nem aktív
            {         
                getdemageAudio.Play(); // A sérülés hang lejátszása

                Lives--; // Egy élet levonása
                LivesUIText.text = Lives.ToString(); // Életek UI frissítése

                // Ha a játékos meghalt
                if (Lives <= 0) 
                {
                    // Game over állapot beállítása
                    GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                    gameObject.SetActive(false); // Játékos eltüntetése
                }

                // Ha 1 élet maradt, értesítjük a PlusHPSpawn scriptet
                if (Lives == 1 && plusHPSpawnScript != null)
                {
                    plusHPSpawnScript.CheckAndSpawnPlusHP(Lives);
                }

                // Indítsuk el a villogást
                StartFlashing();
                StartInvulnerability();
            }
        }
    }
}

    //function to intantiate an explosion
    void PlayExplosion(){
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        //set the position of the explosion
        explosion.transform.position = transform.position;
    }

    // Function to activate PowerShoot
    public void ActivatePowerShoot()
    {
        powerShootTimer = powerShootActiveTime; // Set timer to 15 seconds
        hasPowerShoot = true; // Engedélyezzük a PowerShoot-ot

        // Make extra ship objects visible when PowerShoot is activated
        extra_part01.SetActive(true);
        extra_part02.SetActive(true);
    }

    // Function to deactivate PowerShoot after 15 seconds
    public void DeactivatePowerShoot()
    {
        hasPowerShoot = false; // Disable PowerShoot
        powerShootTimer = 0f; // Reset the timer

        extra_part01.SetActive(false);
        extra_part02.SetActive(false);
    }

    void StartFlashing()
    {
        isFlashing = true; // Indítsuk el a villogást
        flashTimer = flashDuration; // Állítsuk be a villogás időtartamát
        flashCooldown = 0; // Kezdjük el az első villanást
        isVisible = true; // A kezdeti állapot, hogy a hajó látható
        playerSpriteRenderer.enabled = true; // Biztosítjuk, hogy a hajó kezdetben látható
    }

    public void StopFlashing()
    {
        isFlashing = false; // Leállítjuk a villogást
        playerSpriteRenderer.enabled = true; // A hajó látható lesz
        EndInvulnerability();
    }

    public void ResetVisibility()
    {
        playerSpriteRenderer.enabled = true; // Biztosítjuk, hogy a hajó látható legyen
    }

}


