using NUnit.Framework;
using UnityEngine;

public class DirectionTest
{
    private GameObject player; // Tesztelni kívánt objektum
    private PlayerControl PlayerControl; // A script referenciája
    private Camera testCamera; // Teszt kamerát használunk a viewport határokhoz

    [SetUp]
    public void Setup()
    {
        // Hozz létre egy ideiglenes kamerát a teszteléshez
        testCamera = new GameObject("TestCamera").AddComponent<Camera>();
        testCamera.orthographic = true; // Ortografikus kamera
        testCamera.orthographicSize = 5; // Alap méret
        testCamera.transform.position = new Vector3(0, 0, 0); // Pozíció a képernyő közepén

        // Hozz létre egy teszteléshez szükséges Player objektumot
        player = new GameObject("TestPlayer");
        //player.AddComponent<SpriteRenderer>(); // Teszt kedvéért hozzáadunk egy SpriteRenderert
        PlayerControl = player.AddComponent<PlayerControl>();
        PlayerControl.speed = 10f; // Beállítjuk a sebességet
    }

    [TearDown]
    public void Teardown()
    {
        // Teszt után töröljük az objektumokat
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(testCamera.gameObject);
    }

    [Test]
    public void PlayerMovesWithinScreenBounds()
    {
        // Kamera szélének pozíciói
        Vector2 screenMin = testCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenMax = testCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Játékos kiindulási pozíciója
        player.transform.position = Vector2.zero;

        // Hívjuk meg a Move metódust felfelé iránnyal
        Vector2 direction = Vector2.up;
        PlayerControl.Move(direction);

        // Ellenőrizzük, hogy a pozíció a képernyőhatárokon belül van
        Vector2 pos = player.transform.position;
        Assert.IsTrue(pos.x >= screenMin.x && pos.x <= screenMax.x, "Player X position is out of bounds!");
        Assert.IsTrue(pos.y >= screenMin.y && pos.y <= screenMax.y, "Player Y position is out of bounds!");
    }

    [Test]
    public void PlayerMovementUpdatesPositionCorrectly()
    {
        // Játékos kezdő pozíciója
        player.transform.position = Vector2.zero;

        // Irány jobbra
        Vector2 direction = Vector2.right;

        // Elvárt pozíció frissítése
        float deltaTime = Time.deltaTime;
        float expectedX = direction.x * PlayerControl.speed * deltaTime;

        // Hívjuk meg a Move metódust
        PlayerControl.Move(direction);

        // Ellenőrizzük, hogy a pozíció frissült
        Vector2 pos = player.transform.position;
        Assert.AreEqual(expectedX, pos.x, 0.01f, "Player X position did not update correctly!");
        Assert.AreEqual(0, pos.y, 0.01f, "Player Y position should not have changed!");
    }

    [Test]
    public void PlayerStaysWithinClampedBounds()
    {
        // Kamera szélének pozíciói
        Vector2 screenMin = testCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenMax = testCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Állítsuk a játékost a képernyő bal szélére
        player.transform.position = screenMin;

        // Próbáljuk a játékost balra mozgatni
        PlayerControl.Move(Vector2.left);

        // Ellenőrizzük, hogy nem ment ki a képernyőről
        Vector2 pos = player.transform.position;
        Assert.GreaterOrEqual(pos.x, screenMin.x, "Player X position is out of bounds (too far left)!");
        Assert.GreaterOrEqual(pos.y, screenMin.y, "Player Y position is out of bounds (too far down)!");
    }
}