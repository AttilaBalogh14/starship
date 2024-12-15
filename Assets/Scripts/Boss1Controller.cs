using System;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    public float speed = 0f;
    public int Lives = 12;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform player;
    private GameObject scoreUITextGO;
    private bool movingRight = true;
    public GameObject ExplosionGO;

    // Esemény deklaráció
    public static event Action OnBoss1Destroyed;

    void Start()
    {
        movingRight = UnityEngine.Random.value > 0.5f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveDirection = movingRight ? 1f : -1f;
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

        if (transform.position.x >= 4.5f)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -4.5f)
        {
            movingRight = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerBulletTag01") || col.CompareTag("PlayerBulletTag02") || col.CompareTag("PlayerShipTag"))
        {
            Lives--;

            PlayExplosion();

            if (scoreUITextGO != null)
            {
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
            }

            if (Lives <= 0)
            {
                TriggerBoss1DestroyedEvent();
                Destroy(gameObject);
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
            }

            if (col.CompareTag("PlayerBulletTag01") || col.CompareTag("PlayerBulletTag02"))
            {
                Destroy(col.gameObject);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionGO);
        explosion.transform.position = transform.position;
    }

    private void TriggerBoss1DestroyedEvent()
    {
        OnBoss1Destroyed?.Invoke(); // Esemény meghívása az osztályon belül
    }
}
