using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss1SpawnerGO : MonoBehaviour
{
    public GameObject boss1GO;    // Húzd ide a boss1GO prefabot
    public int spawnScore = 1000;   // A pontszám, amelynél a boss megjelenik
    

    private bool hasSpawned = false; // Ellenőrzi, hogy a boss megjelent-e már
    private GameScore gameScore;    // Hivatkozás a GameScore scriptre

    void Start()
    {
        // A GameScore komponens
        gameScore = FindObjectOfType<GameScore>();

        if (gameScore == null)
        {
            Debug.LogError("GameScore script not found in the scene!");
        }

        
        InvokeRepeating("CheckScoreAndSpawnBoss1", 1f, 1f); // Ellenőrzés 1 másodpercenként
    }

    void CheckScoreAndSpawnBoss1()
    {
        // Ellenőrzi, hogy elértük-e a szükséges pontszámot
        if (gameScore != null && gameScore.Score >= spawnScore && !hasSpawned)
        {
            SpawnBoss1();
        }
    }

    void SpawnBoss1()
    {
        // A boss megjelenítése a bal felső sarokban
        Vector3 spawnPosition = GetTopLeftCorner();
        spawnPosition.x += 0.8f; // X tengelyen jobbra mozgatjuk
        spawnPosition.y -= 0.8f; // Y tengelyen lefelé mozgatjuk
        Instantiate(boss1GO, spawnPosition, Quaternion.identity);
    

        hasSpawned = true; // Csak egyszer jelenik meg
        Debug.Log("Boss spawned at: " + spawnPosition);
    }

    Vector3 GetTopLeftCorner()
    {
        // Kamera bal felső sarkának kiszámítása
        Camera mainCamera = Camera.main;
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
        topLeft.z = 0; // 2D játékban a Z-tengely nullázása
        return topLeft;
    }

public bool HasSpawned()
{
    return hasSpawned;
}

   public void ScheduleBoss1Spawner()
{
    hasSpawned = false; // Reseteljük a spawner állapotát
}

    /*public void UnscheduleBoss1Spawner()
    {
        hasSpawned = true; // Eltávolítjuk a spawner-t
    }*/

    public void DestroyActiveBoss1()
{
    GameObject activeBoss1 = GameObject.FindGameObjectWithTag("Boss1ShipTag");
    if (activeBoss1 != null)
    {
        Destroy(activeBoss1);
    }
}
}