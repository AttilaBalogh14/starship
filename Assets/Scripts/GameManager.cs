using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager Instance;

    //Reference to our game objects
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner; //reference to our enemy spawner
    public GameObject asteroidSpawner; //reference to our asteroid spawner
    public GameObject GameOverGO; //reference to the game over image
    public GameObject scoreUITextGO; //reference to the score text UI game object
    public GameObject TimeCounterGO; //reference to the time counter game object
    public GameObject GameTitleGO; // reference to the GameTitleGO
    public GameObject PlusHPSpawnGO; // This is a reference to the GameObject that holds the PlusHPSpawn script
    public GameObject ShieldSpawnGO; // This is a reference to the GameObject that holds the PlusHPSpawn script
    public GameObject ShieldOnPlayer;
    private float shieldduration = 15f;
    private float shieldtimer;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver
    }

    GameManagerState GMState;

    // Singleton initialization
    void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameManager if one already exists
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    //Function to update the game manager state
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
        case GameManagerState.Opening:
            
            //Hide game over 
            GameOverGO.SetActive(false);

            //Display the game title
            GameTitleGO.SetActive(true);

            //set play button visible (active)
            playButton.SetActive(true);

            break;

        case GameManagerState.Gameplay:
            //reset the score
            scoreUITextGO.GetComponent<GameScore>().Score = 0;

            //hide play button on game play state
            playButton.SetActive(false);

            //hide the game title
            GameTitleGO.SetActive(false);

            //set the player visible (active) and init the player lives
            playerShip.GetComponent<PlayerControl>().Init();

            //start enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

            //start asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();


            //start the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

            // Initialize PlusHPSpawn script
            if (PlusHPSpawnGO != null)
            {
                // Ensure PlusHPSpawn component is attached and set the reference
                PlusHPSpawn plusHPSpawnScript = PlusHPSpawnGO.GetComponent<PlusHPSpawn>();
                if (plusHPSpawnScript != null)
                {
                    // Start the timer
                    plusHPSpawnScript.StartTimer();
                }
            }

            // Initialize ShieldSpwan script
            if (PlusHPSpawnGO != null)
            {
                // Ensure ShieldSpawn component is attached and set the reference
                ShieldSpawn ShieldSpawnScript = ShieldSpawnGO.GetComponent<ShieldSpawn>();
                if (ShieldSpawnScript != null)
                {
                    // Start the timer
                    ShieldSpawnScript.StartTimer();
                }
            }
            
            //hide shield 
            ShieldOnPlayer.SetActive(false);
            

            break;
        
        case GameManagerState.GameOver:

            //stop the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

            //stop enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
            //stop asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();
            //display game over
            GameOverGO.SetActive(true);
            //stop timer
            if (PlusHPSpawnGO != null)
            {
                // Ensure PlusHPSpawn component is attached and set the reference
                PlusHPSpawn plusHPSpawnScript = PlusHPSpawnGO.GetComponent<PlusHPSpawn>();
                if (plusHPSpawnScript != null)
                {
                    // Start the timer
                    plusHPSpawnScript.StopTimer();
                }
            }

            if (PlusHPSpawnGO != null)
            {
                // Ensure ShieldSpawn component is attached and set the reference
                ShieldSpawn ShieldSpawnScript = ShieldSpawnGO.GetComponent<ShieldSpawn>();
                if (ShieldSpawnScript != null)
                {
                    // Start the timer
                    ShieldSpawnScript.StopTimer();
                }
            }

            //change game manager state to Opening state after 8 seconds
            Invoke("ChangeToOpeningState", 8f);

            break;
        }
    }

    //Function to set the game manager state
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState ();

    }


    //Our play button will call this action
    //when the user clicks the button
    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();

    }

    //function to change manager state to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }
    
    // Call this method when the player picks up the shield
    public void ActivateShield()
    {
        if (ShieldOnPlayer != null)
        {
            ShieldOnPlayer.SetActive(true);  // Activate the shield
            shieldtimer = shieldduration;   // Reset the shield timer
        }
    }

    void Update()
    {
        // If the shield is active, decrease the shield timer
        if (ShieldOnPlayer.activeSelf)
        {
            shieldtimer -= Time.deltaTime;

            // When the timer runs out, deactivate the shield
            if (shieldtimer <= 0f)
            {
                ShieldOnPlayer.SetActive(false);  // Deactivate the shield
            }
        }
    }
}
