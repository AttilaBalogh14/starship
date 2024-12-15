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
        // Létrehozzuk a Finish Point prefab-et
        finishPointPrefab = new GameObject("FinishPoint1GO");
        finishPointPrefab.tag = "FinishPoint1Tag";

        // Létrehozzuk a spawner objektumot
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
        // Ellenõrizzük, hogy még nincs Finish Point a jelenetben
        Assert.IsNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should not exist before the event is triggered.");

        // Meghívjuk az OnBoss1Destroyed eseményt
        Boss1Controller.OnBoss1Destroyed?.Invoke();

        // Várjunk egy frame-et, hogy az esemény kezelése megtörténjen
        yield return null;

        // Ellenõrizzük, hogy a Finish Point prefab példányosítva lett
        var spawnedFinishPoint = GameObject.FindWithTag("FinishPoint1Tag");
        Assert.IsNotNull(spawnedFinishPoint, "Finish Point should be spawned after the event is triggered.");
        Assert.AreEqual(Vector3.zero, spawnedFinishPoint.transform.position, "Finish Point should be spawned at position (0, 0, 0).");
    }

    [UnityTest]
    public IEnumerator DestroyActiveFinishPoint1_DestroysFinishPoint()
    {
        // Létrehozzuk manuálisan a Finish Point objektumot
        var spawnedFinishPoint = Object.Instantiate(finishPointPrefab, Vector3.zero, Quaternion.identity);

        // Ellenõrizzük, hogy a Finish Point létezik
        Assert.IsNotNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should exist before calling DestroyActiveFinishPoint1.");

        // Meghívjuk az DestroyActiveFinishPoint1 metódust
        spawnerScript.DestroyActiveFinishPoint1();

        // Várjunk egy frame-et, hogy az objektum törlése megtörténjen
        yield return null;

        // Ellenõrizzük, hogy a Finish Point már nem létezik
        Assert.IsNull(GameObject.FindWithTag("FinishPoint1Tag"), "Finish Point should be destroyed after calling DestroyActiveFinishPoint1.");
    }
}

