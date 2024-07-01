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

    private static bool IsAxisCoordinateInRange(ChunkData chunkData, int axisCoordinate) {
        return axisCoordinate >= 0 && axisCoordinate < chunkData.chunkSize;
    }
    private static bool IsYCoordinateInRange(ChunkData chunkData, int yCoordinate) {
        return yCoordinate >= 0 && yCoordinate < chunkData.chunkHeight;
    }

    public static BlockType GetBlock(ChunkData chunkData, Vector3Int pos) {
        if (IsAxisCoordinateInRange(chunkData, pos.x) && IsYCoordinateInRange(chunkData, pos.y) && IsAxisCoordinateInRange(chunkData, pos.z)) {
            int index = GetIndexFromLocalBlockChunkPosition(chunkData, pos);
            return chunkData.blocks[index];
        }

        Vector3Int blockWorldCoord = new(chunkData.worldPos.x + pos.x, chunkData.worldPos.y + pos.y, chunkData.worldPos.z + pos.z);
        return chunkData.worldRef.GetBlockTypeFromBlockPos(blockWorldCoord);
    }

    public static void SetBlockInChunk(ChunkData chunkData, Vector3Int localPosition, BlockType block) {
        if (IsAxisCoordinateInRange(chunkData, localPosition.x) && IsYCoordinateInRange(chunkData, localPosition.y) && IsAxisCoordinateInRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromLocalBlockChunkPosition(chunkData, localPosition);
            chunkData.blocks[index] = block;
        }
        else
        {
            throw new Exception("Need to ask World for appropiate chunk");
        }
    }

    private static int GetIndexFromLocalBlockChunkPosition(ChunkData chunkData, Vector3Int pos) {
        int x = pos.x;
        int y = pos.y;
        int z = pos.z;

        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }
    
    public static Vector3Int WorldBlockCoordToLocalBlockCoord(ChunkData chunkData, Vector3Int blockWorldCoord) {
        return new Vector3Int {
            x = blockWorldCoord.x - chunkData.worldPos.x,
            y = blockWorldCoord.y - chunkData.worldPos.y,
            z = blockWorldCoord.z - chunkData.worldPos.z
        };
    }


    public static MeshData GetChunkMeshData(ChunkData chunkData) {
        MeshData meshData = new MeshData(true);

        void BlockIteratorFunc(Vector3Int blockPosition) {
            int blockIndex = GetIndexFromLocalBlockChunkPosition(chunkData, blockPosition);
            BlockType blockType = chunkData.blocks[blockIndex];

            BlockHelper.AddBlockMeshData(meshData, chunkData, blockPosition, blockType);
        }

        LoopThroughChunkBlocks(chunkData, BlockIteratorFunc);

        return meshData;
    }
    
    public static Vector3Int ConvertWorldBlockCoordToWorldChunkCoord(World world, Vector3Int pos) {
        return new Vector3Int {
            x = Mathf.FloorToInt(pos.x / (float)world.GetChunkSize()) * world.GetChunkSize(),
            y = Mathf.FloorToInt(pos.y / (float)world.GetChunkHeight()) * world.GetChunkHeight(),
            z = Mathf.FloorToInt(pos.z / (float)world.GetChunkSize()) * world.GetChunkSize()
        };
    }
}