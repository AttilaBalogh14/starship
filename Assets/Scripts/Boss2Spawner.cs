using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss2SpawnerGO : MonoBehaviour
{
    public GameObject boss2GO;    // Húzd ide a boss1GO prefabot
    public int spawnScore = 10000;   // A pontszám, amelynél a boss megjelenik

    private bool hasSpawned = false; // Ellenőrzi, hogy a boss megjelent-e már
    private GameScore gameScore;    // Hivatkozás a GameScore scriptre

    void Start()
    {
        // Keresd meg a GameScore komponenst
        gameScore = FindObjectOfType<GameScore>();

        if (gameScore == null)
        {
            Debug.LogError("GameScore script not found in the scene!");
        }

        
        InvokeRepeating("CheckScoreAndSpawnBoss2", 1f, 1f); // Ellenőrzés 1 másodpercenként
    }

    void CheckScoreAndSpawnBoss2()
    {
        // Ellenőrzi, hogy elértük-e a szükséges pontszámot
        if (gameScore != null && gameScore.Score >= spawnScore && !hasSpawned)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        // A boss megjelenítése a bal felső sarokban
        Vector3 spawnPosition = GetTopLeftCorner();
        spawnPosition.x += 0.8f; // X tengelyen jobbra mozgatjuk
        spawnPosition.y -= 0.8f; // Y tengelyen lefelé mozgatjuk
        Instantiate(boss2GO, spawnPosition, Quaternion.identity);

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

 public void ScheduleBoss2Spawner()
{
    hasSpawned = false; // Reseteljük a spawner állapotát
}
    /*public void UnscheduleBoss2Spawner()
    {
        hasSpawned = true; // Eltávolítjuk a spawner-t
    }*/

    public void DestroyActiveBoss2()
    {
        GameObject activeBoss2 = GameObject.FindGameObjectWithTag("Boss2ShipTag");
        if (activeBoss2 != null)
        {
            Destroy(activeBoss2);
        }
    }
}