using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ne felejts el hivatkozni a UI-ra
using System.IO; // Hozzáadva, hogy elérhesd a Path osztályt
using TMPro;
using UnityEngine.SceneManagement; // TextMeshPro namespace


public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager Instance;
    //Reference to our game objects
    public GameObject playButton;
    public GameObject quitButton;
    public GameObject playerShip;
    public GameObject enemySpawner; //reference to our enemy spawner
    public GameObject Boss1SpawnerGO;//reference to our boss1 spawner
    public GameObject Boss2SpawnerGO;//reference to our boss2 spawner
    public GameObject Boss3SpawnerGO;//reference to our boss3 spawner
    public GameObject FinishPoint1SpawnerGO;//reference to our FinishPoint1 spawner
    public GameObject FinishPoint2SpawnerGO;//reference to our inishPoint2spawner
    public GameObject asteroidSpawner; //reference to our asteroid spawner
    public GameObject GameOverGO; //reference to the game over image
    public GameObject scoreUITextGO; //reference to the score text UI game object
    public GameObject TimeCounterGO; //reference to the time counter game object
    private TimeCounter timeCounterScript;
    public GameObject GameTitleGO; // reference to the GameTitleGO
    private SceneController sceneControllerScript;

    private HighscoreManager highscoreManagerScript;
    GameObject ScoreTextGO;
    private GameScore scoreScript; // Reference to GameScore script to access the score


    // Reference to AudioSources
    public AudioSource backgroundMusic;
    public AudioSource deathMusic;
    public GameObject PlusHPSpawnGO; // This is a reference to the GameObject that holds the PlusHPSpawn script
    public GameObject ShieldSpawnGO; // This is a reference to the GameObject that holds the ShieldSpawn script
    public GameObject PowerShootSpawnGO; // This is a reference to the Gameobject that holfs the PowerShootSpawn script
    public GameObject ShieldOnPlayer;
    
    private float savedTimeElapsed;
    private float shieldduration = 15f;
    private float shieldtimer;
    public int levelNumber;
    public bool isShieldActive = false;  // Külön változó a pajzs aktív állapotához

    // Referencia a szövegdobozhoz
    public TextMeshProUGUI levelDescriptionText;  // UI szöveg, amely megjeleníti a szint leírását
    public TextMeshProUGUI levelTitle;  // UI szöveg, amely megjeleníti a szint leírását

    // JSON fájl beolvasása
    private string jsonFilePath = "level_descriptions.json"; // A JSON fájl elérési útja

    [System.Serializable]
    public class Level
    {
        public int level;
        public string title;
        public string description;
    }

    [System.Serializable]
    public class LevelList
    {
        public Level[] levels;
    }

    private LevelList levelList;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver
    }

    GameManagerState GMState;

    // Singleton initialization
    void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameManager if one already exists
        }

        ShieldOnPlayer.SetActive(false);
    }

        void LoadLevelDescriptions()
        {
            // Betölti a JSON fájlt
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                levelList = JsonUtility.FromJson<LevelList>(json);

                if (levelList != null && levelList.levels.Length > 0)
                {

                    // Itt megjelenítjük a szint leírását és címét a szövegdobozban
                    levelDescriptionText.text = levelList.levels[getLevelNumber()].description;
                    levelTitle.text = levelList.levels[getLevelNumber()].title;
                }
                else
                {
                    Debug.LogError("A szint leírása nem található vagy hibás a JSON!");
                }
            }
            else
            {
                Debug.LogError("JSON fájl nem található!");
            }
        }

    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.Opening;

        // Ensure music states are correct at start
        if (backgroundMusic != null)
            backgroundMusic.Play();
        if (deathMusic != null)
            deathMusic.Stop();

        LoadLevelDescriptions();  // Betöltjük a szint információkat

        ShieldOnPlayer.SetActive(false);

        highscoreManagerScript = FindObjectOfType<HighscoreManager>();

        if (highscoreManagerScript != null)
        {
            highscoreManagerScript.LoadHighscore(); // Betöltjük a legmagasabb pontszámot
        }

        ScoreTextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        scoreScript = scoreUITextGO.GetComponent<GameScore>();
        //scoreScript.ResetScore();

        // Inicializáljuk a TimeCounter scriptet
        timeCounterScript = TimeCounterGO.GetComponent<TimeCounter>();

        sceneControllerScript = FindAnyObjectByType<SceneController>();

        // Betölti a mentett adatokat
        GameScore.Instance.LoadScore();
        TimeCounter.Instance.LoadTime();



    }

    // Game over event
    public void OnGameOver()
    {
        int currentScore = scoreScript.Score; // Az aktuális pontszám lekérése
        highscoreManagerScript.UpdateHighscore(currentScore); // Frissítjük a legmagasabb pontszámot

        // Egyéb logikák a játék vége után (pl. játékos elpusztulása, UI frissítése)

        // Nullázzuk a pontszámot és az időt a játék végén
        GameScore.Instance.ResetScore();
        TimeCounter.Instance.ResetTime(); 

        // Töröljük a mentett adatokat, amikor a játékos meghal
        ClearSavedData();


    }

    public void setLevelNumber(int LevelNumber){
        levelNumber = LevelNumber;
    }

    public int getLevelNumber(){
        return levelNumber;
    }

    // Szint leírásának megjelenítése
    /*void DisplayLevelDescription(int levelIndex)
    {
        if (levelDescriptionText != null && levelDescriptionText.gameObject.activeSelf )
        {
            // Az adott szint leírásának keresése és megjelenítése
            foreach (var level in levelList.levels)
            {
                if (level.level == levelIndex)
                {
                    levelDescriptionText.text = level.description;
                    return;
                }
            }
        }
        else
        {
            Debug.LogError("levelDescriptionText nem érvényes.");
        }
    }
    */
     // Play gomb kattintás eseménye
    public void OnPlayButtonClick()
    {
        // Tüntessük el a szövegdobozt, amikor a Play gombra kattintanak
        levelDescriptionText.gameObject.SetActive(false);
        levelTitle.gameObject.SetActive(false);
        
        // A játékot elindítjuk itt
        // GameManager state frissítés, stb.

        // Játék újraindításakor visszaállítjuk az időt
    }

    //Function to update the game manager state
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
        case GameManagerState.Opening:
            
            // Hide game over 
            GameOverGO.SetActive(false);

            // Display the game title
            GameTitleGO.SetActive(true);

            // Set play button visible (active)
            playButton.SetActive(true);

            quitButton.SetActive(true);

            levelDescriptionText.gameObject.SetActive(true);
            levelTitle.gameObject.SetActive(true);

            
            // Play background music
            if (backgroundMusic != null && !backgroundMusic.isPlaying)
                backgroundMusic.Play();
            
            if (deathMusic != null && deathMusic.isPlaying)
                deathMusic.Stop();

            // Példa: Szint 1 leírásának megjelenítése
            //DisplayLevelDescription(levelNumber);
            //setLevelNumber(levelNumber +1);
            //DisplayLevelDescription(getLevelNumber());

            ShieldOnPlayer.SetActive(false);

            break;

        case GameManagerState.Gameplay:
            // Reset the score
            //scoreUITextGO.GetComponent<GameScore>().Score = 0;

            // Hide play button on game play state
            playButton.SetActive(false);

            quitButton.SetActive(false);

            // Hide the game title
            GameTitleGO.SetActive(false);

            // Set the player visible (active) and init the player lives
            playerShip.GetComponent<PlayerControl>().Init();

            // Start enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

            //start asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();  
            
            //start boss1 spawner
            //Boss1SpawnerGO.GetComponent<Boss1SpawnerGO>().ScheduleBoss1Spawner();

            // Start the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

            ShieldOnPlayer.SetActive(false);

            // Initialize ShieldSpwan script
            if (ShieldSpawnGO != null)
            {
                // Ensure ShieldSpawn component is attached and set the reference
                ShieldSpawn ShieldSpawnScript = ShieldSpawnGO.GetComponent<ShieldSpawn>();
                if (ShieldSpawnScript != null)
                {
                    // Start the timer
                    ShieldSpawnScript.StartTimer();
                }
            }

            // Initialize PowerShootSpawn script
            if (PowerShootSpawnGO != null)
            {
                // Ensure PowerShootSpawn component is attached and set the reference
                PowerShootSpawn PowerShootSpawnScript = PowerShootSpawnGO.GetComponent<PowerShootSpawn>();
                if (PowerShootSpawnScript != null)
                {
                    // Start the timer
                    PowerShootSpawnScript.StartTimer();
                }
            }
            

            // Initialize PlusHPSpawn script
            if (PlusHPSpawnGO != null)
            {
                // Ensure PlusHPSpawn component is attached and set the reference
                PlusHPSpawn plusHPSpawnScript = PlusHPSpawnGO.GetComponent<PlusHPSpawn>();
                if (plusHPSpawnScript != null)
                {
                    // Start the timer
                    plusHPSpawnScript.StartTimer();
                }
            }
           
            //hide shield 
            ShieldOnPlayer.SetActive(false);
            
            // Play background music if not already playing
            if (backgroundMusic != null && !backgroundMusic.isPlaying)
                backgroundMusic.Play();
            
            if (deathMusic != null && deathMusic.isPlaying)
                deathMusic.Stop();

            break;
        
        case GameManagerState.GameOver:

            OnGameOver();


            // Stop the time counter
            TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

            // Stop enemy spawner
            enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
            //destroy boss1
            Boss1SpawnerGO.GetComponent<Boss1SpawnerGO>().DestroyActiveBoss1();
            //destroy boss2
            Boss2SpawnerGO.GetComponent<Boss2SpawnerGO>().DestroyActiveBoss2();
            //destroy boss3
            Boss3SpawnerGO.GetComponent<Boss3SpawnerGO>().DestroyActiveBoss3();
            //destroy finishpoint1
            FinishPoint1SpawnerGO.GetComponent<FinishPoint1SpawnerGO>().DestroyActiveFinishPoint1();
            //destroy finishpoint2
            FinishPoint2SpawnerGO.GetComponent<FinishPoint2SpawnerGO>().DestroyActiveFinishPoint2();
            //stop asteroid spawner
            asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

            // Display game over
            GameOverGO.SetActive(true);
            playerShip.GetComponent<PlayerControl>().DeactivatePowerShoot();
            //stop timer
            if (PlusHPSpawnGO != null)
            {
                // Ensure PlusHPSpawn component is attached and set the reference
                PlusHPSpawn plusHPSpawnScript = PlusHPSpawnGO.GetComponent<PlusHPSpawn>();
                if (plusHPSpawnScript != null)
                {
                    // Start the timer
                    plusHPSpawnScript.StopTimer();
                }
            }

            if (ShieldSpawnGO != null)
            {
                // Ensure ShieldSpawn component is attached and set the reference
                ShieldSpawn ShieldSpawnScript = ShieldSpawnGO.GetComponent<ShieldSpawn>();
                if (ShieldSpawnScript != null)
                {
                    // Start the timer
                    ShieldSpawnScript.StopTimer();
                }
            }

            if (PowerShootSpawnGO != null)
            {
                // Ensure PowerShootSpawn component is attached and set the reference
                PowerShootSpawn PowerShootSpawnScript = PowerShootSpawnGO.GetComponent<PowerShootSpawn>();
                if (PowerShootSpawnScript != null)
                {
                    // Start the timer
                    PowerShootSpawnScript.StopTimer();
                }
            }

            highscoreManagerScript.UpdateHighscore(ScoreTextGO.GetComponent<GameScore>().Score);
            
            // Stop background music
            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            // Play death music
            if (deathMusic != null)
                deathMusic.Play();

            // Change game manager state to Opening state after 8 seconds
            Invoke("ChangeToOpeningState", 8f);

            break;
        }
    }


    //Function to set the game manager state
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    //Our play button will call this action
    //when the user clicks the button
    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
        // Ne hagyjuk, hogy a hajó villogjon, amikor újraindul a játék
        playerShip.GetComponent<PlayerControl>().StopFlashing();  // Leállítjuk a villogást a PlayerControl scriptben
        playerShip.GetComponent<PlayerControl>().ResetVisibility();  // Biztosítjuk, hogy a hajó látható legyen

        // Betöltjük a szint leírását újra
        LoadLevelDescriptions();  

        // Töröljük a mentett adatokat új játék kezdésekor
        ClearSavedData();

        
    }

    //Function to change manager state to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);

        // Betöltjük a szint leírását újra
        LoadLevelDescriptions();  
    }
    // Call this method when the player picks up the shield
    public void ActivateShield()
    {
        if (ShieldOnPlayer != null)
        {
            ShieldOnPlayer.SetActive(true);  // Activate the shield
            shieldtimer = shieldduration;   // Reset the shield timer

            isShieldActive = true;
        }
    }

    void Update()
    {
        // Ellenőrizzük, hogy a ShieldOnPlayer létezik
        if (ShieldOnPlayer != null && ShieldOnPlayer.activeSelf)
        {
            shieldtimer -= Time.deltaTime;

            // Ha az időzítő lejárt, deaktiváljuk a pajzsot
            if (shieldtimer <= 0f)
            {
                ShieldOnPlayer.SetActive(false);
                isShieldActive = false;
            }
        }
        else if (ShieldOnPlayer == null)
        {
            Debug.LogWarning("ShieldOnPlayer objektum meg lett semmisítve.");
            ShieldOnPlayer = null;
        }
     
    }

    public void QuitGame()
    {
        GameScore.Instance.ResetScore();
        TimeCounter.Instance.ResetTime();

        // Töröljük a mentett adatokat, amikor a játékos kilép
        ClearSavedData();

        int currentScore = scoreScript.Score; // Az aktuális pontszám lekérése
        highscoreManagerScript.UpdateHighscore(currentScore); // Frissítjük a legmagasabb pontszámot

        Application.Quit();
    }

    public void ClearSavedData()
    {
        PlayerPrefs.DeleteKey("SavedScore"); // A pontszám törlése
        PlayerPrefs.DeleteKey("SavedTime");  // Az idő törlése
    }

    // Ez a függvény fogja betölteni a főmenü jelenetet
    public void GoToMainMenu()
    {
        // A "MainMenu" nevű jelenetet betölti
        SceneManager.LoadScene("Main Menu");
        GameScore.Instance.ResetScore();
        TimeCounter.Instance.ResetTime();

        int currentScore = scoreScript.Score; // Az aktuális pontszám lekérése
        highscoreManagerScript.UpdateHighscore(currentScore); // Frissítjük a legmagasabb pontszámot
    }

    }
