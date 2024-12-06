using UnityEngine;

public class FinishPoint : MonoBehaviour
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
