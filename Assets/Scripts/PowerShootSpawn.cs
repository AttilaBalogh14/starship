using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShootSpawn : MonoBehaviour
{
    public GameObject powerShootGO;
    private float spawnTimer = 0f; // Időzítő változó
    private bool hasSpawned = false; // Jelzi, hogy létrejött-e már a PowerShoot objektum
    private bool gameStarted = false; // Jelzi, hogy a játék elkezdődött
    private float nextspawntime;
    public PlayerControl playerControl; // Hivatkozás a PlayerControl scriptre

    // Start is called before the first frame update
    void Start()
    {
        // Induláskor a timer 0-ra van állítva, és még nem spawnoltunk.
        spawnTimer = 0f;
        hasSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return; // Ha a játék még nem indult el, ne számoljunk

        // Időzítő növelése minden frame-ben
        spawnTimer += Time.deltaTime;


        // ha elértük a random generált időzítést és még nem spawnoltunk Shield objektumot
        if (spawnTimer >= nextspawntime && !hasSpawned)
        {
            hasSpawned = true; // Beállítjuk, hogy már spawnoltunk
            SpawnPowerShoot(); // Létrehozzuk a PlusHP objektumot
            nextspawntime = Random.Range(50, 65);
            spawnTimer = 0f;
        }
    }

    public void SpawnPowerShoot()
    {
        ChooseRandomPosition(); // Véletlenszerű hely kiválasztása
        spawnTimer = 0f; // Időzítő visszaállítása
        hasSpawned = false; // Spawnolás után visszaállítjuk a státuszt, hogy újraindulhasson a timer, ha szükséges
    }

    void ChooseRandomPosition()
    {
        // A képernyő bal alsó sarka (viewport koordináta)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // A képernyő jobb felső sarka (viewport koordináta)
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Véletlenszerű pozíció a képernyőn belül
        Vector2 randomPosition = new Vector2(Random.Range(min.x, max.x), max.y);

        // Létrehozzuk az élet objektumot a véletlenszerű pozícióban
        Instantiate(powerShootGO, randomPosition, Quaternion.identity);
    }

     public void StartTimer()
    {
        gameStarted = true; // Beállítjuk, hogy a játék elindult
        nextspawntime = Random.Range(50f, 65f); // Az első spawn időpontját beállítjuk
    }

    public void StopTimer()
    {
        gameStarted = false; // Beállítjuk, hogy a játék elindult
        spawnTimer = 0f;
    }
}
