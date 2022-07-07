using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckBehaviour
{
    void CheckBlocks(List<Block> blocksList, IRuleset ruleset, int blockAmountWidth, int blockAmountHeight);
}
