using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);

        //GameScore.Instance.SaveScore();  // Hívd meg a GameManager SaveScore metódust

    }

    
    public static void NextLevel()
    {
        // Mentjük el a pontszámot a következő szint előtt
        //GameScore.Instance.SaveScore();  // Ha menteni szeretnéd
        //TimeCounter.Instance.SaveTime(); // Elmentjük az időt is


        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
