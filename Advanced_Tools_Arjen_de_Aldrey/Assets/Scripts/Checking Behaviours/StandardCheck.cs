﻿using System.Collections.Generic;
using UnityEngine;

public class StandardCheck : ICheckBehaviour
{
    public void CheckBlocks(List<Block> blocksList, IRuleset ruleset)
    {
        foreach (Block blockToCheck in blocksList)
        {
            blockToCheck.checkNeighbours();
        }
        foreach (Block blockToCheck in blocksList)
        {
            blockToCheck.ToggleAlive(ruleset.CheckAlive(blockToCheck));
        }
    }
}
