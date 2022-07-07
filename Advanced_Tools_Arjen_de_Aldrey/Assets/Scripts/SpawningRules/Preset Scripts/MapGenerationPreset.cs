using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationPreset : ScriptableObject
{
    public ISpawnRuleset spawnRuleset; 

    public MapGenerationPreset()
    {
        spawnRuleset = new StandardSpawningRuleset();
    }
}
