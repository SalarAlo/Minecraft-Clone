using System;
using UnityEngine;

public static class Chunk
{
    /// <summary>
    /// Performs an action foreach block withing a chunk
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="actionToPerform"></param>
    public static void LoopThroughChunkBlocks(ChunkData chunkData, Action<Vector3Int> actionToPerform)
    {
        for (int index = 0; index < chunkData.blocks.Length; index++)
        {
            var position = GetLocalChunkPostitionFromIndex(chunkData, index);
            actionToPerform(position);
        }
    }

    /// <summary>
    /// converts a index into a local chunk Position
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private static Vector3Int GetLocalChunkPostitionFromIndex(ChunkData chunkData, int index)
    {
        int x = index % chunkData.chunkSize;
        int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = index / (chunkData.chunkSize * chunkData.chunkHeight);
        return new Vector3Int(x, y, z);
    }

    /// <summary>
    /// Checks to see if the axis Coordinate (x or z) is withing the chunks dimensions
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="axisCoordinate"></param>
    /// <returns></returns>
    private static bool IsAxisCoordinateInRange(ChunkData chunkData, int axisCoordinate)
    {
        if (axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize)
            return false;

        return true;
    }

    /// <summary>
    /// Checks to see if the y Coordinate is within the chunks dimensions
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="ycoordinate"></param>
    /// <returns></returns>
    private static bool IsYCoordinateInRange(ChunkData chunkData, int ycoordinate)
    {
        if (ycoordinate < 0 || ycoordinate >= chunkData.chunkHeight)
            return false;

        return true;
    }

    /// <summary>
    /// Retrieves the block type at a given local position within the chunk. if no pos not in range of the chunk it will search for the block in world ref of the chunk passed in
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static BlockType GetBlock(ChunkData chunkData, Vector3Int pos) {
        if (IsAxisCoordinateInRange(chunkData, pos.x) && IsYCoordinateInRange(chunkData, pos.y) && IsAxisCoordinateInRange(chunkData, pos.z)) {
            int index = GetIndexFromLocalBlockChunkPosition(chunkData, pos);
            return chunkData.blocks[index];
        }

        return chunkData.worldRef.GetBlockTypeFromBlockPos(new(chunkData.worldPos.x + pos.x, chunkData.worldPos.y + pos.y, chunkData.worldPos.z + pos.z));
    }

    /// <summary>
    /// Sets the type of a block withing a chunk
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="localPosition"></param>
    /// <param name="block"></param>
    /// <exception cref="Exception"></exception>
    public static void SetBlockInChunk(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
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

    /// <summary>
    /// Converts a 3d position into a index (1d)
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static int GetIndexFromLocalBlockChunkPosition(ChunkData chunkData, Vector3Int pos) {
        int x = pos.x;
        int y = pos.y;
        int z = pos.z;
        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }

    /// <summary>
    /// converts a world block position to a local block position for its chunk.
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="blockWorldCoord"></param>
    /// <returns></returns>
    public static Vector3Int WorldBlockCoordToLocalBlockCoord(ChunkData chunkData, Vector3Int blockWorldCoord) {
        return new Vector3Int {
            x = blockWorldCoord.x - chunkData.worldPos.x,
            y = blockWorldCoord.y - chunkData.worldPos.y,
            z = blockWorldCoord.z - chunkData.worldPos.z
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chunkData"></param>
    /// <returns></returns>
    public static MeshData GetChunkMeshData(ChunkData chunkData) {
        MeshData meshData = new MeshData(true);

        void BlockIteratorFunc(Vector3Int position) {
            int blockIndex = GetIndexFromLocalBlockChunkPosition(chunkData, position);
            BlockHelper.AddBlockMeshData(meshData, chunkData, position, chunkData.blocks[blockIndex]);
        }

        LoopThroughChunkBlocks(chunkData, BlockIteratorFunc);

        return meshData;
    }


    /// <summary>
    /// converts a world block pos to a world chunk pos
    /// </summary>
    /// <param name="world"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3Int ConvertWorldBlockCoordToWorldChunkCoord(World world, Vector3Int pos) {
        return new Vector3Int {
            x = Mathf.FloorToInt(pos.x / (float)world.GetChunkSize()) * world.GetChunkSize(),
            y = Mathf.FloorToInt(pos.y / (float)world.GetChunkHeight()) * world.GetChunkHeight(),
            z = Mathf.FloorToInt(pos.z / (float)world.GetChunkSize()) * world.GetChunkSize()
        };
    }
}