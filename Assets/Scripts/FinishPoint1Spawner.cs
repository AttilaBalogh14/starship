using UnityEngine;


public class FinishPoint1SpawnerGO : MonoBehaviour
{
    public GameObject FinishPoint1GO;    // Finish Point prefab


    void Update()
    {
        // Ha a Boss1 megsemmisült és még nem jelent meg a FinishPoint, akkor megjeleníthetjük
        if (GetComponent<Boss1Controller>().IsDead()==true)
        {
            SpawnFinishPoint1();
        }
    }

    void SpawnFinishPoint1()
    {
        // FinishPoint megjelenítése a kívánt helyen
        Instantiate(FinishPoint1GO, new Vector3(0, 0, 0), Quaternion.identity); 
        Debug.Log("Finish Point spawned!");
    }
}