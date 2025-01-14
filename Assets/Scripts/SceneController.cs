using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static void LoadScene(int sceneIndex)
    {
        // Mentsd el a pontszámot és az időt az aktuális szint előtt
        GameScore.Instance.SaveScore();
        TimeCounter.Instance.SaveTime();

        SceneManager.LoadScene(sceneIndex);

        //GameScore.Instance.SaveScore();  // Hívd meg a GameManager SaveScore metódust

    }

    
    public static void NextLevel()
    {
        // Mentsd el a pontszámot és az időt az aktuális szint előtt
        GameScore.Instance.SaveScore();
        TimeCounter.Instance.SaveTime();

        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static string GetCurrentSceneName()
    {
        return (string) SceneManager.GetActiveScene().name;
    }
}
