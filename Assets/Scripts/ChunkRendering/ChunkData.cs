using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChunkData 
{
    // 1d array to represent 3d space becouse more performant
    public BlockType[] blocks;
    public int chunkSize = 16;
    public int chunkHeight = 100;
    public World worldRef;
    public Vector3Int worldPos;

    public bool modifiedByPlayer = false;


    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPos) {
        this.chunkSize = chunkSize;
        this.chunkHeight = chunkHeight;
        worldRef = world;
        this.worldPos = worldPos;
        blocks = new BlockType[chunkSize*chunkHeight*chunkSize];
    }
}
