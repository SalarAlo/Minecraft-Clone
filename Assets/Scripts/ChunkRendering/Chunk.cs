using System;
using UnityEngine;

public static class Chunk
{

    public static void LoopThroughChunkBlocks(ChunkData chunkData, Action<Vector3Int> actionToPerform)
    {
        for (int index = 0; index < chunkData.blocks.Length; index++)
        {
            var position = GetLocalChunkPostitionFromIndex(chunkData, index);
            actionToPerform(position);
        }
    }

    private static Vector3Int GetLocalChunkPostitionFromIndex(ChunkData chunkData, int index)
    {
        int x = index % chunkData.chunkSize;
        int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = index / (chunkData.chunkSize * chunkData.chunkHeight);
        return new Vector3Int(x, y, z);
    }

    //in chunk coordinate system
    private static bool InRange(ChunkData chunkData, int axisCoordinate)
    {
        if (axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize)
            return false;

        return true;
    }

    //in chunk coordinate system
    private static bool InRangeHeight(ChunkData chunkData, int ycoordinate)
    {
        if (ycoordinate < 0 || ycoordinate >= chunkData.chunkHeight)
            return false;

        return true;
    }

    public static BlockType GetBlockFromLocalChunkCoordinates(ChunkData chunkData, Vector3Int pos)
    {
        if (InRange(chunkData, pos.x) && InRangeHeight(chunkData, pos.y) && InRange(chunkData, pos.z))
        {
            int index = GetIndexFromLocalChunkPosition(chunkData, pos);
            return chunkData.blocks[index];
        }

        return chunkData.worldRef.GetBlockFromChunkCoords(chunkData, new(chunkData.worldPos.x + pos.x, chunkData.worldPos.y + pos.y, chunkData.worldPos.z + pos.z));
    }

    public static void SetBlockInChunk(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
        if (InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromLocalChunkPosition(chunkData, localPosition);
            chunkData.blocks[index] = block;
        }
        else
        {
            throw new Exception("Need to ask World for appropiate chunk");
        }
    }

    private static int GetIndexFromLocalChunkPosition(ChunkData chunkData, Vector3Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        int z = pos.z;
        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }

    public static Vector3Int WorldPositionToChunkPosition(ChunkData chunkData, Vector3Int pos)
    {
        return new Vector3Int
        {
            x = pos.x - chunkData.worldPos.x,
            y = pos.y - chunkData.worldPos.y,
            z = pos.z - chunkData.worldPos.z
        };
    }

    public static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new MeshData(true);

        LoopThroughChunkBlocks(chunkData, (pos) => meshData = BlockHelper.GetMeshData(chunkData, pos, meshData, chunkData.blocks[GetIndexFromLocalChunkPosition(chunkData, pos)]));

        return meshData;
    }

    // Gets Chunk Pos of any block position
    public static Vector3Int ChunkPositionFromBlockCoords(World world, Vector3Int pos)
    {
        return new Vector3Int {
            x = Mathf.FloorToInt(pos.x / (float)world.GetChunkSize()) * world.GetChunkSize(),
            y = Mathf.FloorToInt(pos.y / (float)world.GetChunkHeight()) * world.GetChunkHeight(),
            z = Mathf.FloorToInt(pos.z / (float)world.GetChunkSize()) * world.GetChunkSize()
        };
    }
}