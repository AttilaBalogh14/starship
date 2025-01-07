using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject AsteroidGO; // Az aszteroida prefab, amit instantiálunk
    float maxSpawnRateInSeconds = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

        //function spawn an enemy
    void SpawnAsteroid(){
        
        // spawn enemy random side of the screen
        choseSide();


        //Schedule when to spawn next enemy
        ScheduleNextAsteroidSpawn();

    }

    public int choseSide(){

        //this is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        //this is the top-right point if the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

        int side = Random.Range(0, 2);

        //instantiate an enemy

        GameObject anAsteroid = (GameObject)Instantiate (AsteroidGO);

        if(side==0){ //asteroid from top
            anAsteroid.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
        } else if (side == 1) {  //asteroid from left
            anAsteroid.transform.position = new Vector2(min.x, Random.Range(min.y, max.y));
        } else if (side==2){  // asteroid from right
            anAsteroid.transform.position = new Vector2(max.x, Random.Range(min.y, max.y));
        }

        // Az oldal információ átadása az aszteroida mozgásának
        //anAsteroid.GetComponent<AsteroidControl>().SetDirectionBasedOnSide(side);

        return side;
    }

    void ScheduleNextAsteroidSpawn(){
        float spawnInNSaecond;
        if (maxSpawnRateInSeconds > 1f){
            //pick a number between 1 and maxSpawnRateInSeconds
            spawnInNSaecond = Random.Range(3f, maxSpawnRateInSeconds);
        }
        else
            spawnInNSaecond = 3f;

        Invoke("SpawnAsteroid", spawnInNSaecond);
    }

    //Function to increase the difficulty of the game
    void IncreaseSpawnRate(){
        if (maxSpawnRateInSeconds > 1f){
            maxSpawnRateInSeconds--;
        }

        if (maxSpawnRateInSeconds == 3f){
            CancelInvoke("IncreaseSpawnRate");
        }
    }

    //Function to start enemy spawner
    public void ScheduleAsteroidSpawner()
    {
        //reset max spawn rate
        maxSpawnRateInSeconds = 7f;

        Invoke("SpawnAsteroid", maxSpawnRateInSeconds);

        //increase spawn rate every 30 seconds
        InvokeRepeating("IncreaseSpawnRate", 0f, 15f);
    }
    

    //function to stop enemy spawner
    public void UnscheduleAsteroidSpawner()
    {
        CancelInvoke("SpawnAsteroid");
        CancelInvoke("IncreaseSpawnRate");
    }
}
