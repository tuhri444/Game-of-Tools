using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnRuleset
{
    void GenerateMap(float blockWidth,
        float blockHeight,
        int blockAmountWidth,
        int blockAmountHeight,
        GameObject blockPrefab,
        ref Block[,] blocks,
        ref List<Block> blocksList,
        Transform parentObject);
}
