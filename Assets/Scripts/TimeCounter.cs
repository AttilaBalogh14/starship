using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter Instance;

    public Text timeUI; //reference to the time counter UI text

    float startTime; //the time when the user clicks on play
    public float ellapsedTime; //the ellapsed time after the user clicks on play
    bool startCounter; //flag to start the counter

    int minutes;
    int seconds;

    public float savedTime; // Store time when transitioning between levels

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Ne törlődjön le a következő szintre váltáskor
        }
        else
        {
            Destroy(gameObject);  // Ha már létezik, töröld az új példányt
        }
        RestoreTime();
    }

    // Start is called before the first frame update
    void Start()
    {
        startCounter = false;
        ellapsedTime = 0f + savedTime;  // Kezdő idő érték 0

        //get the Text UI component from this GameObject
        timeUI = GetComponent<Text>();    
    }

    //Function to start the time counter
    public void StartTimeCounter()
    {
        startTime = Time.time - savedTime;// Use saved time to continue from where it left off
        startCounter = true;
    }

    //function to stop the time counter
    public void StopTimeCounter()
    {
        startCounter = false;
    }

    // Function to save the elapsed time (for when switching levels)
    public void SaveTime()
    {
        PlayerPrefs.SetFloat("SavedTime", ellapsedTime); // Elmentjük az eltelt időt
        PlayerPrefs.Save();  // Mentjük a változásokat
        StopTimeCounter(); // Stop counting the time
    }

    public void RestoreTime()
{
    if (PlayerPrefs.HasKey("SavedTime")) // Ha létezik mentett idő
    {
        savedTime = PlayerPrefs.GetFloat("SavedTime");
        ellapsedTime = savedTime; // Beállítjuk az eltelt időt a mentett értékre
    }
    else
    {
        savedTime = 0f;  // Ha nincs mentett idő, akkor 0-ról indul
        ellapsedTime = 0f;
    }
}

    // Function to reset the time (when the player dies or quits)
    public void ResetTime()
    {
        savedTime = 0f; // Reset saved time
        ellapsedTime = 0f; // Reset the elapsed time
        timeUI.text = "00:00"; // Update the UI to show "00:00"
    }

    // Update is called once per frame
    void Update()
    {
        if(startCounter)
        {
            //compute the ellapsed time
            ellapsedTime = Time.time - startTime;

            minutes = (int)ellapsedTime / 60; //get the minutes
            seconds = (int)ellapsedTime % 60; //get the seconds

            //update the time counter UI text
            timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
