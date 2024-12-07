using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    
    public TextMeshProUGUI highscoreText; // UI elem a legmagasabb pontszám megjelenítésére

    private HighscoreManager highscoreManager; // A HighscoreManager referencia


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
  
    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        // Megkeressük a HighscoreManager-t a jelenetben
        highscoreManager = FindObjectOfType<HighscoreManager>();

        if (highscoreManager != null)
        {
            highscoreManager.LoadHighscore(); // Betöltjük a legmagasabb pontszámot
            DisplayHighscore(); // Megjelenítjük a legmagasabb pontszámot
        }
        else
        {
            Debug.LogError("HighscoreManager nem található a jelenetben!");
        }

    }

    // A főmenüben megjeleníti a legmagasabb pontszámot
    public void DisplayHighscore()
    {
        int highscore = highscoreManager.GetHighscore();
        highscoreText.text = "Highest Score: " + highscore.ToString();
    }

    
}
