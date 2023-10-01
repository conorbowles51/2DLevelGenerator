using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Generation/Simple Random Walk Preset")]
public class SimpleRandomWalkPreset : ScriptableObject
{
    public int iterations = 10;
    public int walkLength = 10;
    public bool useRandomStartPosition = true;
}
