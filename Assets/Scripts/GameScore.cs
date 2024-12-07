using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{   
    public Text scoreTextUI;

    public static GameScore Instance;

    int score = 0;

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
        LoadScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        //get the text ui component of this GameObject
        scoreTextUI = GetComponent<Text>();
        LoadScore();  // Betöltjük a mentett pontszámot
    }

    

    //function to update the score text UI
    public  void UpdateScoreTextUi()
    {
        string scoreStr = string.Format ("{0:0000000}", score);
        scoreTextUI.text = scoreStr;
    }

    public void SaveScore()
    {
        // Mentjük a pontszámot
        PlayerPrefs.SetInt("score", score);  // "Highscore" a kulcs, score a mentett adat
        PlayerPrefs.Save();  // Mentés
    }

    public void LoadScore()
    {
        // Betöltjük a pontszámot (ha létezik)
        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score");  // Betöltjük a mentett pontszámot
        }
        else
        {
            score = 0;  // Ha nincs mentett pontszám, akkor 0
        }

        UpdateScoreTextUi();  // UI frissítése
    }

    public void OnLevelComplete()
    {
        // A pontszám mentése a szint végén
        SaveScore();
    }

    // Pontszám nullázása
        public void ResetScore()
        {
            score = 0;
            UpdateScoreTextUi();  // UI frissítése
        }

}
