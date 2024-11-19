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
    public GameObject asteroidSpawner; //reference to our asteroid spawner
    public GameObject GameOverGO; //reference to the game over image
    public GameObject scoreUITextGO; //reference to the score text UI game object
    public GameObject TimeCounterGO; //reference to the time counter game object
    public GameObject GameTitleGO; // reference to the GameTitleGO

    // Reference to AudioSources
    public AudioSource backgroundMusic;
    public AudioSource deathMusic;

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

        // Ensure music states are correct at start
        if (backgroundMusic != null)
            backgroundMusic.Play();
        if (deathMusic != null)
            deathMusic.Stop();
    }

    //Function to update the game manager state
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
        case GameManagerState.Opening:
            
            // Hide game over 
            GameOverGO.SetActive(false);

            // Display the game title
            GameTitleGO.SetActive(true);

            // Set play button visible (active)
            playButton.SetActive(true);

            // Play background music
            if (backgroundMusic != null && !backgroundMusic.isPlaying)
                backgroundMusic.Play();
            
            if (deathMusic != null && deathMusic.isPlaying)
                deathMusic.Stop();

            break;

        case GameManagerState.Gameplay:
            // Reset the score
            scoreUITextGO.GetComponent<GameScore>().Score = 0;

            // Hide play button on game play state
            playButton.SetActive(false);

            // Hide the game title
            GameTitleGO.SetActive(false);

            // Set the player visible (active) and init the player lives
            playerShip.GetComponent<PlayerControl>().Init();

            // Start enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

            // Start asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();

            // Start the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

            // Play background music if not already playing
            if (backgroundMusic != null && !backgroundMusic.isPlaying)
                backgroundMusic.Play();
            
            if (deathMusic != null && deathMusic.isPlaying)
                deathMusic.Stop();

            break;
        
        case GameManagerState.GameOver:

            // Stop the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

            // Stop enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
            // Stop asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

            // Display game over
            GameOverGO.SetActive(true);

            // Stop background music
            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            // Play death music
            if (deathMusic != null)
                deathMusic.Play();

            // Change game manager state to Opening state after 8 seconds
            Invoke("ChangeToOpeningState", 8f);

            break;
        }
    }

    //Function to set the game manager state
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    //Our play button will call this action
    //when the user clicks the button
    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    //Function to change manager state to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }
}

