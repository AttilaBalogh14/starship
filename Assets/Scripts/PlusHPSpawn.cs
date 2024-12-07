using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusHPSpawn : MonoBehaviour
{
    public GameObject PlusHPGO; // A plusz hp prefab, amit instantiálunk
    private float spawnTimer = 0f; // Időzítő változó
    private bool hasSpawned = false; // Jelzi, hogy létrejött-e már a PlusHP objektum
    private bool gameStarted = false; // Jelzi, hogy a játék elkezdődött

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

        // Ha 1 perc (60 másodperc) eltelt és még nem spawnoltunk PlusHP objektumot
        if (spawnTimer >= 60f && !hasSpawned)
        {
            hasSpawned = true; // Beállítjuk, hogy már spawnoltunk
            SpawnPlusHP(); // Létrehozzuk a PlusHP objektumot
            
        }
    }

    // A PlayerControl élet számot kapunk át
    public void CheckAndSpawnPlusHP(int currentLives)
    {
        if (currentLives == 1 && !hasSpawned) // Ha 1 élet maradt, és még nem spawnoltunk
        {
            hasSpawned = true; // Jelzés, hogy létrejött
            SpawnPlusHP(); // Ha 1 élet maradt, spawnoljuk a PlusHP-t
            
        }
    }

    public void SpawnPlusHP()
    {
        ChooseRandomPosition(); // Véletlenszerű hely kiválasztása
        spawnTimer = 0f; // Időzítő visszaállítása
        hasSpawned = false; // Spawnolás után visszaállítjuk a státuszt, hogy újraindulhasson a timer, ha szükséges
    }

    // Véletlenszerű hely a képernyőn, ahol az élet megjelenik
    void ChooseRandomPosition()
    {
        // A képernyő bal alsó sarka (viewport koordináta)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // A képernyő jobb felső sarka (viewport koordináta)
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Véletlenszerű pozíció a képernyőn belül
        Vector2 randomPosition = new Vector2(Random.Range(min.x, max.x), max.y);

        // Létrehozzuk az élet objektumot a véletlenszerű pozícióban
        Instantiate(PlusHPGO, randomPosition, Quaternion.identity);
    }

    // Indítja el az időzítőt
    public void StartTimer()
    {
        gameStarted = true; // Beállítjuk, hogy a játék elindult
    }

    public void StopTimer()
    {
        gameStarted = false; // Beállítjuk, hogy a játék elindult
        spawnTimer = 0f;
    }
}
