using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Chunk 
{
    public static void LoopThroughChunk(ChunkData chunkData, Action<int, int, int> action) {
        for(int i = 0; i < chunkData.blocks.Length;  i++) {
            Vector3Int pos = GetPositionFromIndex(chunkData, i);
            action(pos.x, pos.y, pos.z);
        }
    }

    private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int i) {
        int x = i % chunkData.chunkSize;
        int y = (i / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = i / (chunkData.chunkSize * chunkData.chunkHeight);

        return new(x, y, z);
    }
    
    public static BlockType GetBlockFromChunkCoords(ChunkData chunkData, Vector3Int pos) {
        if(!InRange(chunkData, pos)) throw new Exception("Need to ask world for appropiate Chunk");

        int index = GetIndexFromPosition(chunkData, pos);
        return chunkData.blocks[index];
    }

    private static bool InRangeAxis(ChunkData chunkData, int axisCoord) =>
        axisCoord >= 0 && axisCoord < chunkData.chunkSize;
    private static bool InRangeHeight(ChunkData chunkData, int yCoord) => 
        yCoord >= 0 && yCoord < chunkData.chunkHeight;
    private static bool InRange(ChunkData chunkData, Vector3Int pos) => 
        InRangeAxis(chunkData, pos.x) && InRangeHeight(chunkData, pos.y) && InRangeAxis(chunkData, pos.z);
    
    public static void SetBlock(ChunkData chunkData, Vector3Int localPos, BlockType block) {
        if(InRange(chunkData, localPos)) {
            int index = GetIndexFromPosition(chunkData, localPos);
            chunkData.blocks[index] = block;
        } else {
            throw new Exception("Need to ask world for appropiate chunk");
        }
    }

    private static int GetIndexFromPosition(ChunkData chunkData, Vector3Int pos) {
        return pos.x + chunkData.chunkSize * pos.y + chunkData.chunkSize * chunkData.chunkHeight * pos.z;
    }

    public static Vector3Int GetBlockInChunkCoords(ChunkData chunkData, Vector3Int pos) {
        return new Vector3Int() {
            x = pos.x - chunkData.worldPos.x,
            y = pos.y - chunkData.worldPos.y,
            z = pos.z - chunkData.worldPos.z,
        };
    }

    public static MeshData GetChunkMeshData(ChunkData chunkData) {
        MeshData meshData = new(true);

        // TODO: Fill later

        return meshData;
    }
}
