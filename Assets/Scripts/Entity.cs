using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    private Vector3 spawnPosition;
    private PrimitiveType objectType;
    private string name;

    public Entity(string name, Vector3 spawnPosition, PrimitiveType objectType)
    {
        this.name = name;
        this.spawnPosition = spawnPosition;
        this.objectType = objectType;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }

    public string GetName()
    {
        return name;
    }

    public virtual GameObject Spawn()
    {
        GameObject gameObject = GameObject.CreatePrimitive(objectType);
        gameObject.transform.position = spawnPosition;
        gameObject.name = name;
        return gameObject;
    }
}
