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
        // Hozz létre egy teszt jelenetet, és töltsd be
        yield return LoadTestScene(TestSceneName);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Az eredeti jelenet visszatöltése, ha szükséges
        yield return LoadTestScene(TestSceneName);
    }

    [UnityTest]
    public IEnumerator LoadScene_LoadsSpecifiedScene()
    {
        // Ellenõrizzük az aktuális jelenetet
        Assert.AreEqual(TestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the test scene.");

        // Hívjuk meg a LoadScene metódust
        SceneController.LoadScene(5); // Feltételezzük, hogy a 2. jelenet indexe 5

        // Várjunk, hogy a jelenet betöltõdjön
        yield return new WaitForSeconds(0.1f);

        // Ellenõrizzük, hogy a második jelenet betöltõdött
        Assert.AreEqual(SecondTestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the second test scene.");
    }

    [UnityTest]
    public IEnumerator NextLevel_LoadsNextScene()
    {
        // Ellenõrizzük az aktuális jelenetet
        Assert.AreEqual(TestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the test scene.");

        // Hívjuk meg a NextLevel metódust
        SceneController.NextLevel();

        // Várjunk, hogy a következõ jelenet betöltõdjön
        yield return new WaitForSeconds(0.1f);

        // Ellenõrizzük, hogy a második jelenet betöltõdött
        Assert.AreEqual(SecondTestSceneName, SceneManager.GetActiveScene().name, "The current scene should be the second test scene.");
    }

    private IEnumerator LoadTestScene(string sceneName)
    {
        // Ellenõrizzük, hogy a jelenet már be van-e töltve
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
