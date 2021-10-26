using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : Entity
{
    private GameObject healthBarPrefab;
    private Slider healthBar;
    private int health;
    private int maxHealth;

    private int damagePerSecond;

    public Target(string name, int health, Vector3 spawnPosition, PrimitiveType objectType , GameObject healthBarPrefab, int damagePerSecond) : base(name,spawnPosition, objectType)
    {
        this.damagePerSecond = damagePerSecond;
        this.healthBarPrefab = healthBarPrefab;
        maxHealth = health;
        this.health = maxHealth;
    }

    public override GameObject Spawn()
    {
        GameObject gameObject = base.Spawn();

        Vector3 cubePosition = GetSpawnPosition();
        Vector3 healthBarPosition = cubePosition;
        GameObject healthBarGameObject = Object.Instantiate(healthBarPrefab, healthBarPosition, Quaternion.identity);

        healthBar = healthBarGameObject.GetComponentInChildren<Slider>();
        healthBarGameObject.transform.SetParent(gameObject.transform);

        return gameObject;
    }

    public void Hit()
    {
        health -= damagePerSecond;
        healthBar.value = health;
        if (IsCubeDied())
        {
            RespawnCube();
        }
    }

    private bool IsCubeDied()
    {
        if (health < 0)
        {
            return true;
        }
        return false;
    }

    private void RespawnCube()
    {
        health = maxHealth;
    }
}
