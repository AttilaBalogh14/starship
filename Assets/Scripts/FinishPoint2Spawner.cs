using UnityEngine;

public class FinishPoint2SpawnerGO : MonoBehaviour
{
    public GameObject FinishPoint2GO; // Finish Point prefab

    private bool finishPointSpawned = false;

    void OnEnable()
    {
        // Feliratkozás az eseményre
        Boss2Controller.OnBoss2Destroyed += SpawnFinishPoint2;
    }

    void OnDisable()
    {
        // Leiratkozás az eseményről
        Boss2Controller.OnBoss2Destroyed -= SpawnFinishPoint2;
    }

    void SpawnFinishPoint2()
    {
        if (!finishPointSpawned) // Ellenőrizzük, hogy már megjelent-e
        {
            Instantiate(FinishPoint2GO, new Vector3(0, 0, 0), Quaternion.identity);
            finishPointSpawned = true;
            Debug.Log("Finish Point spawned!");
        }
    }

   public void DestroyActiveFinishPoint2()
    {
    GameObject activeFinishPoint2 = GameObject.FindGameObjectWithTag("FinishPoint2Tag");
    if (activeFinishPoint2 != null)
    {
        Destroy(activeFinishPoint2);
    }
}

}