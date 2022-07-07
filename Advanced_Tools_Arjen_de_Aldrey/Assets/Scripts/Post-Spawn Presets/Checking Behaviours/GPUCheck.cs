using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUCheck : ICheckBehaviour
{
    private ComputeShader neighbourCheckingShader;

    public GPUCheck()
    {
    }

    public void CheckBlocks(List<Block> blocksList, IRuleset ruleset, int blockAmountWidth, int blockAmountHeight)
    {
        if (!neighbourCheckingShader) neighbourCheckingShader = SpawnBlocks.instance.GPUShader;

        int[] states = new int[blocksList.Count];
        for (int i = 0; i < blocksList.Count; i++)
        {
            states[i] = blocksList[i].alive ? 1 : 0;
        }

        int totalSize = sizeof(int);
        ComputeBuffer oldStateBuffer = new ComputeBuffer(blocksList.Count, totalSize);
        oldStateBuffer.SetData(states);
        ComputeBuffer newStateBuffer = new ComputeBuffer(blocksList.Count, totalSize);

        neighbourCheckingShader.SetBuffer(0, "oldStates", oldStateBuffer);
        neighbourCheckingShader.SetBuffer(0, "newStates", newStateBuffer);
        neighbourCheckingShader.SetInt("stateListSize", blocksList.Count);
        neighbourCheckingShader.SetInt("stateListWidth", blockAmountWidth);
        neighbourCheckingShader.SetInt("stateListHeight", blockAmountHeight);
        neighbourCheckingShader.SetInt("screenWidth", Screen.width);
        neighbourCheckingShader.SetInt("screenHeight", Screen.height);

        neighbourCheckingShader.SetTexture(0, "renderTexture", SpawnBlocks.instance.renderTarget);

        neighbourCheckingShader.Dispatch(0, blocksList.Count/32, 1, 1);

        newStateBuffer.GetData(states);
        
        for (int i = 0; i < blocksList.Count; i++)
        {
            blocksList[i].ToggleAlive(states[i] == 1 ? true : false);
        }
    }
}
