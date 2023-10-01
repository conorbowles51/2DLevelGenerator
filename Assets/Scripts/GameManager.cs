using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int entityCount = 0;

    private void Awake() 
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }    

        instance = this;
    }

    public int AssignNewEntityID()
    {
        return entityCount++;
    }
}

// Keeps track of entities, objects, score, etc
