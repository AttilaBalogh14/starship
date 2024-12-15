using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneControllerTests
{
    private const string TestSceneName = "TestScene";
    private const string SecondTestSceneName = "SecondTestScene";

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Hozz l�tre egy teszt jelenetet, �s t�ltsd be
        yield return LoadTestScene(TestSceneName);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Az eredeti jelenet visszat�lt�se, ha sz�ks�ges
        yield return LoadTestScene(TestSceneName);
    }

    [UnityTest]
    public IEnumerator LoadScene_LoadsSpecifiedScene()
    {
        // Ellen�rizz�k az aktu�lis jelenetet
        Assert.AreEqual(TestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the test scene.");

        // H�vjuk meg a LoadScene met�dust
        SceneController.LoadScene(5); // Felt�telezz�k, hogy a 2. jelenet indexe 5

        // V�rjunk, hogy a jelenet bet�lt�dj�n
        yield return new WaitForSeconds(0.1f);

        // Ellen�rizz�k, hogy a m�sodik jelenet bet�lt�d�tt
        Assert.AreEqual(SecondTestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the second test scene.");
    }

    [UnityTest]
    public IEnumerator NextLevel_LoadsNextScene()
    {
        // Ellen�rizz�k az aktu�lis jelenetet
        Assert.AreEqual(TestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the test scene.");

        // H�vjuk meg a NextLevel met�dust
        SceneController.NextLevel();

        // V�rjunk, hogy a k�vetkez� jelenet bet�lt�dj�n
        yield return new WaitForSeconds(0.1f);

        // Ellen�rizz�k, hogy a m�sodik jelenet bet�lt�d�tt
        Assert.AreEqual(SecondTestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the second test scene.");
    }

    private IEnumerator LoadTestScene(string sceneName)
    {
        // Ellen�rizz�k, hogy a jelenet m�r be van-e t�ltve
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            var loadSceneOp = SceneManager.LoadSceneAsync(sceneName);
            while (!loadSceneOp.isDone)
            {
                yield return null;
            }
        }
    }
}
