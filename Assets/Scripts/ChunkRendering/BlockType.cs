using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType 
{
    // Nothing: Only if the blockPos is in a chunk that doesnt exist
    Nothing,
    Air,
    Grass_Dirt,
    Dirt,
    Grass_Stone,
    Stone,
    TreeTrunk,
    TreeLeafsSolid,
    TreeLeafsTransparent,
    Water,
    Sand
}
