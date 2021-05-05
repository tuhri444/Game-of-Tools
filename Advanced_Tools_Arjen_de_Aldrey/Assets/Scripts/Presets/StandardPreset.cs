using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StandardPreset", menuName = "GameOfLife/BehaviourPreset", order = 1)]
public class StandardPreset : ScriptableObject
{
    public IRuleset ruleset;
    public ICheckBehaviour checkBehaviour;

    public StandardPreset()
    {
        ruleset = new StandardRuleset();
        checkBehaviour = new StandardCheck();
    }
}
