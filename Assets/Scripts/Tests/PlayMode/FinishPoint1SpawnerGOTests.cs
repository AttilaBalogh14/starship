using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FinishPoint1SpawnerGOTests
{
    private GameObject spawnerObject;
    private FinishPoint1SpawnerGO spawnerScript;
    private GameObject finishPointPrefab;

    [SetUp]
    public void SetUp()
    {
        // L�trehozzuk a Finish Point prefab-et
        finishPointPrefab = new GameObject("FinishPoint1GO");
        finishPointPrefab.tag = "FinishPoint1Tag";

        // L�trehozzuk a spawner objektumot
        spawnerObject = new GameObject("FinishPoint1SpawnerGO");
        spawnerScript = spawnerObject.AddComponent<FinishPoint1SpawnerGO>();
        spawnerScript.FinishPoint1GO = finishPointPrefab;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(finishPointPrefab);
        Object.DestroyImmediate(spawnerObject);
    }

    [UnityTest]
    public IEnumerator SpawnFinishPoint1_SpawnsFinishPointOnEvent()
    {
        // Ellen�rizz�k, hogy m�g nincs Finish Point a jelenetben
        Assert.IsNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should not exist before the event is triggered.");

        // Megh�vjuk az OnBoss1Destroyed esem�nyt
        Boss1Controller.OnBoss1Destroyed?.Invoke();

        // V�rjunk egy frame-et, hogy az esem�ny kezel�se megt�rt�njen
        yield return null;

        // Ellen�rizz�k, hogy a Finish Point prefab p�ld�nyos�tva lett
        var spawnedFinishPoint = GameObject.FindWithTag("FinishPoint1Tag");
        Assert.IsNotNull(spawnedFinishPoint, "Finish Point should be spawned after the event is triggered.");
        Assert.AreEqual(Vector3.zero, spawnedFinishPoint.transform.position, "Finish Point should be spawned at position (0, 0, 0).");
    }

    [UnityTest]
    public IEnumerator DestroyActiveFinishPoint1_DestroysFinishPoint()
    {
        // L�trehozzuk manu�lisan a Finish Point objektumot
        var spawnedFinishPoint = Object.Instantiate(finishPointPrefab, Vector3.zero, Quaternion.identity);

        // Ellen�rizz�k, hogy a Finish Point l�tezik
        Assert.IsNotNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should exist before calling DestroyActiveFinishPoint1.");

        // Megh�vjuk az DestroyActiveFinishPoint1 met�dust
        spawnerScript.DestroyActiveFinishPoint1();

        // V�rjunk egy frame-et, hogy az objektum t�rl�se megt�rt�njen
        yield return null;

        // Ellen�rizz�k, hogy a Finish Point m�r nem l�tezik
        Assert.IsNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should be destroyed after calling DestroyActiveFinishPoint1.");
    }
}

