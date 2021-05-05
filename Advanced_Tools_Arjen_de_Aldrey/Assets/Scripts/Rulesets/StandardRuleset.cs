using UnityEngine;

public class StandardRuleset : IRuleset
{
    public bool CheckAlive(Block blockToCheck)
    {
        bool isAlive = blockToCheck.alive;
        int aliveNeighbours = blockToCheck.aliveNeighbours;

        if (!blockToCheck.alive && aliveNeighbours == 3)
            isAlive = true;
        else if (blockToCheck.alive && (aliveNeighbours == 2 || aliveNeighbours == 3))
            isAlive = true;
        else
            isAlive = false;

        return isAlive;
    }
}
