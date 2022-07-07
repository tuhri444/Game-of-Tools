using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeighbourCheckingPreset : ScriptableObject
{
    public IRuleset ruleset;
    public ICheckBehaviour checkBehaviour;

    public NeighbourCheckingPreset()
    {
        ruleset = new StandardRuleset();
        checkBehaviour = new StandardCheck();
    }
}
