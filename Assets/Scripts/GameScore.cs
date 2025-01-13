using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{   
    public Text scoreTextUI;

    public static GameScore Instance;

    int score = 0;

    public int prevlevelscore;

    public int Score
    {
        get
        {
            return this.score;
        }
        set
        {
            this.score = value;
            UpdateScoreTextUi();
        }
    }

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
    }

    // Start is called before the first frame update
    void Start()
    {
        //get the text ui component of this GameObject
        scoreTextUI = GetComponent<Text>();
    }

    

    //function to update the score text UI
    public  void UpdateScoreTextUi()
    {
        string scoreStr = string.Format ("{0:0000000}", score);
        scoreTextUI.text = scoreStr;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("SavedScore", score);  // Mentés
        prevlevelscore=score;
    }

    public void LoadScore()
    {
        score = PlayerPrefs.GetInt("SavedScore", 0);  // Betöltés, alapértelmezett 0
        UpdateScoreTextUi();
    }

    // Pontszám nullázása
        public void ResetScore()
        {
            score = 0;
            UpdateScoreTextUi();  // UI frissítése
        }

}
