using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRuleset
{
    bool CheckAlive(Block blockToCheck);
}
