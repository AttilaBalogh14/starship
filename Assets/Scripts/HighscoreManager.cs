using System.IO;
using UnityEngine;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    private string highscoreFilePath = "highscore.json"; // Fájl elérési útja
    private int highscore = 0;

    [System.Serializable]
    public class HighscoreData
    {
        public int highscore;
    }

    // Betölti a legmagasabb pontszámot a JSON fájlból
    public void LoadHighscore()
    {
        string filePath = Path.Combine(Application.persistentDataPath, highscoreFilePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(json);
            highscore = data.highscore;
        }
        else
        {
            // Ha még nincs fájl, alapértelmezett érték
            highscore = 0;
            SaveHighscore();
        }
    }

    // Ment egy új legmagasabb pontszámot a JSON fájlba
    public void SaveHighscore()
    {
        HighscoreData data = new HighscoreData();
        data.highscore = highscore;

        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, highscoreFilePath);
        File.WriteAllText(filePath, json);
    }

    // Frissíti a legmagasabb pontszámot, ha a mostani score nagyobb
    public void UpdateHighscore(int score)
    {
        if (score > highscore)
        {
            highscore = score;
            SaveHighscore();
        }
    }

    // Visszaadja a legmagasabb pontszámot
    public int GetHighscore()
    {
        return highscore;
    }
}
