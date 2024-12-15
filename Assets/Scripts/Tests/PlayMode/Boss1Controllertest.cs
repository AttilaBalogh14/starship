using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Boss1Controllertest
{
    private GameObject boss;
    private Boss1Controller bossController;
    private float initialPositionX;

    [SetUp]
    public void SetUp()
    {
        // Létrehozzuk a GameObject-et és hozzáadjuk a Boss1Controller scriptet
        boss = new GameObject("Boss1");
        bossController = boss.AddComponent<Boss1Controller>();

        // Inicializáljuk a mozgási sebességet
        bossController.speed = 5f;

        // Kezdeti pozíciót beállítunk
        boss.transform.position = new Vector3(0f, 0f, 0f);

        // Megjegyezzük az inicializálás előtti X pozíciót
        initialPositionX = boss.transform.position.x;
    }

    [TearDown]
    public void TearDown()
    {
        // Töröljük a GameObject-et, ha a teszt véget ér
        Object.DestroyImmediate(boss);
    }

    [Test]
    public void Move_MovesRight_WhenStartingDirectionIsRight()
    {
        // Beállítjuk a movingRight értékét, hogy jobbra mozogjon
        bossController.movingRight = true;

        // Elmentjük az aktuális pozíciót
        float initialX = boss.transform.position.x;

        // Meghívjuk a Move metódust
        bossController.Move();

        // Ellenőrizzük, hogy a pozíció nőtt-e (jobbra mozgott)
        Assert.Greater(boss.transform.position.x, initialX, "Boss should move to the right when movingRight is true.");
    }

    [Test]
    public void Move_MovesLeft_WhenStartingDirectionIsLeft()
    {
        // Beállítjuk a movingRight értékét, hogy balra mozogjon
        bossController.movingRight = false;

        // Elmentjük az aktuális pozíciót
        float initialX = boss.transform.position.x;

        // Meghívjuk a Move metódust
        bossController.Move();

        // Ellenőrizzük, hogy a pozíció csökkent-e (balra mozgott)
        Assert.Less(boss.transform.position.x, initialX, "Boss should move to the left when movingRight is false.");
    }

    [Test]
    public void Move_ChangesDirection_WhenHittingScreenEdge()
    {
        // A mozgás sebességét csökkentjük, hogy a teszt gyorsabban véget érjen
        bossController.speed = 1f;

        // Beállítjuk a boss pozícióját a jobb szélre
        boss.transform.position = new Vector3(4.5f, 0f, 0f);
        bossController.movingRight = true;

        // Meghívjuk a Move metódust, hogy elérje a jobb szélt
        bossController.Move();

        // Ellenőrizzük, hogy a movingRight értéke false-ra váltott
        Assert.IsFalse(bossController.movingRight, "Boss should change direction when hitting the right screen edge.");

        // Most beállítjuk balra, hogy balra menjen
        bossController.movingRight = false;
        boss.transform.position = new Vector3(-4.5f, 0f, 0f);

        // Meghívjuk a Move metódust, hogy elérje a bal szélt
        bossController.Move();

        // Ellenőrizzük, hogy a movingRight értéke true-ra váltott
        Assert.IsTrue(bossController.movingRight, "Boss should change direction when hitting the left screen edge.");
    }
}