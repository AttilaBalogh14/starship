using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishPoint2Controller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShipTag"))
        {
            //go to next level
            SceneController.NextLevel();
        }
    }

}