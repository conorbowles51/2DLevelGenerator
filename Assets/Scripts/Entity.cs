using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    public int entityID = -1;
    public int maxHealth = 100;

    private int currentHealth = 0;

    private GameManager gameManager;

    protected void InitalizeEntity()
    {
        gameManager = GameManager.instance;

        entityID = gameManager.AssignNewEntityID();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}

// Base class for all entities, i.e player, enemies, bosses

// GameManager can assign an ID to each entity which will be useful
// for ensuring things like an enemy archer can't damage itself with
// its own arrows
