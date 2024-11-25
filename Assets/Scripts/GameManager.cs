using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Reference to our game objects
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner; //reference to our enemy spawner
   // public GameObject Boss1Spawner; //reference to our boss1 spawner
    public GameObject asteroidSpawner; //reference to our asteroid spawner
    public GameObject GameOverGO; //reference to the game over image
    public GameObject scoreUITextGO; //reference to the score text UI game object
    public GameObject TimeCounterGO; //reference to the time counter game object
    public GameObject GameTitleGO; // reference to the GameTitleGO

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver
    }

    GameManagerState GMState;



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
            
            //start boss1 spawner
            //Boss1Spawner.GetComponent<Boss1SpawnerGO>().ScheduleBoss1Spawner();


            //start the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

            break;
        
        case GameManagerState.GameOver:

            //stop the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

            //stop enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
            //stop boss1 spawner
            //Boss1Spawner.GetComponent<Boss1SpawnerGO>().UnscheduleBoss1Spawner();
            //stop asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();
            //display game over
            GameOverGO.SetActive(true);
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
}
