using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GPUPreset", menuName = "GameOfLife/GPUCheckingPreset", order = 3)]
public class GPUCheckingPreset : NeighbourCheckingPreset
{
    public GPUCheckingPreset()
    {
        ruleset = new StandardRuleset();
        checkBehaviour = new GPUCheck();
    }
}
