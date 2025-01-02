using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss3SpawnerGO : MonoBehaviour
{
    public GameObject boss3GO;    // Húzd ide a boss3GO prefabot
    public int spawnScore = 20000;   // A pontszám, amelynél a boss megjelenik

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

        
        InvokeRepeating("CheckScoreAndSpawnBoss3", 1f, 1f); // Ellenőrzés 1 másodpercenként
    }

    void CheckScoreAndSpawnBoss3()
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
        spawnPosition.x += 1.3f; // X tengelyen jobbra mozgatjuk
        spawnPosition.y -= 1.3f; // Y tengelyen lefelé mozgatjuk
        Instantiate(boss3GO, spawnPosition, Quaternion.identity);

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

   /* public void UnscheduleBoss3Spawner()
    {
        hasSpawned = true; // Eltávolítjuk a spawner-t
    }*/

    public void DestroyActiveBoss3()
{
    GameObject activeBoss3 = GameObject.FindGameObjectWithTag("Boss3ShipTag");
    if (activeBoss3 != null)
    {
        Destroy(activeBoss3);
    }
}
}


