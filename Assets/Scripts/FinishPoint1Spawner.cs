using UnityEngine;

public class FinishPoint1SpawnerGO : MonoBehaviour
{
    public GameObject FinishPoint1GO; // Finish Point prefab

    private bool finishPointSpawned = false;

    void OnEnable()
    {
        // Feliratkozás az eseményre
        Boss1Controller.OnBoss1Destroyed += SpawnFinishPoint1;
    }

    void OnDisable()
    {
        // Leiratkozás az eseményről
        Boss1Controller.OnBoss1Destroyed -= SpawnFinishPoint1;
    }

    void SpawnFinishPoint1()
    {
        if (!finishPointSpawned) // Ellenőrizzük, hogy már megjelent-e
        {
            Instantiate(FinishPoint1GO, new Vector3(0, 0, 0), Quaternion.identity);
            finishPointSpawned = true;
            Debug.Log("Finish Point spawned!");
        }
    }
    public void DestroyActiveFinishPoint1()
    {
    GameObject activeFinishPoint1 = GameObject.FindGameObjectWithTag("FinishPoint1Tag");
    if (activeFinishPoint1 != null)
    {
        Destroy(activeFinishPoint1);
    }
}

}