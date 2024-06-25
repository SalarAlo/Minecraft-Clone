using System;
using UnityEngine;

public static class Chunk 
{
    /// <summary>
    ///     perform a action foreach block within a chunk 
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="action"></param>
    public static void LoopThroughChunk(ChunkData chunkData, Action<Vector3Int> action) {
        for(int i = 0; i < chunkData.blocks.Length;  i++) {
            Vector3Int pos = GetChunkPositionFromIndex(chunkData, i);
            action(pos);
        }
    }

    /// <summary>
    ///     converts index to 3d local chunk space
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private static Vector3Int GetChunkPositionFromIndex(ChunkData chunkData, int i) {
        int x = i % chunkData.chunkSize;
        int y = (i / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = i / (chunkData.chunkSize * chunkData.chunkHeight);

        return new(x, y, z);
    }

    /// <summary>
    ///     retrieve a block of a local position within a chunk 
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static BlockType GetBlockFromChunkCoords(ChunkData chunkData, Vector3Int pos) {
        if(!InRange(chunkData, pos)) {
            return chunkData.worldRef.GetBlockFromChunkCoords(chunkData, new Vector3Int(chunkData.worldPos.x + pos.x, chunkData.worldPos.y + pos.y, chunkData.worldPos.y + pos.y));
        }

        int index = GetIndexFromChunkPosition(chunkData, pos);
        return chunkData.blocks[index];
    }

    /// <summary>
    ///     check if an axis coordinate (x or z) is in floor space of chunk
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="axisCoord"></param>
    /// <returns></returns>
    private static bool InRangeAxis(ChunkData chunkData, int axisCoord) =>
        axisCoord >= 0 && axisCoord < chunkData.chunkSize;
    
    /// <summary>
    ///     check if yCoord is in range of chunkData height
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="yCoord"></param>
    /// <returns></returns>
    private static bool InRangeHeight(ChunkData chunkData, int yCoord) => 
        yCoord >= 0 && yCoord < chunkData.chunkHeight;
    
    /// <summary>
    ///     check if a pos is in Range of the chunk dimensions
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static bool InRange(ChunkData chunkData, Vector3Int pos) => 
        InRangeAxis(chunkData, pos.x) && InRangeHeight(chunkData, pos.y) && InRangeAxis(chunkData, pos.z);
    
    /// <summary>
    ///     Set Block on Position to a certain block type
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="localPos"></param>
    /// <param name="block"></param>
    /// <exception cref="Exception"></exception>
    public static void SetBlock(ChunkData chunkData, Vector3Int localPos, BlockType block) {
        if(InRange(chunkData, localPos)) {
            int index = GetIndexFromChunkPosition(chunkData, localPos);
            chunkData.blocks[index] = block;
        } else {
            throw new Exception("Need to ask world for appropiate chunk");
        }
    }

    /// <summary>
    ///     Local Chunk Pos to Index
    /// </summary>
    /// <param name="chunkData">chunk Data</param>
    /// <param name="pos">chunk Pos</param>
    /// <returns></returns>
    private static int GetIndexFromChunkPosition(ChunkData chunkData, Vector3Int pos) {
        // x pos normal, y pos * chunkSize (becouse x-len == chunk size) to move up a row, z-pos * width * height
        return (pos.x) + (chunkData.chunkSize * pos.y) + (chunkData.chunkSize * chunkData.chunkHeight * pos.z);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="chunkData"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3Int GetBlockInChunkCoords(ChunkData chunkData, Vector3Int pos) {
        return new Vector3Int() {
            x = pos.x - chunkData.worldPos.x,
            y = pos.y - chunkData.worldPos.y,
            z = pos.z - chunkData.worldPos.z,
        };
    }

    public static MeshData GetChunkMeshData(ChunkData chunkData) {
        MeshData meshData = new(true);

        LoopThroughChunk(chunkData, (pos) => {
            meshData = BlockHelper.GetMeshData(chunkData, pos, meshData, chunkData.blocks[GetIndexFromChunkPosition(chunkData, pos)]);
        });

        return meshData;
    }

    internal static Vector3Int GlobalPositionToChunkPosition(World world, Vector3Int pos) {
        return new() {
            x = Mathf.FloorToInt(pos.x / (float)world.GetChunkSize()) * world.GetChunkSize(),
            y = Mathf.FloorToInt(pos.y / (float)world.GetChunkHeight()) * world.GetChunkHeight(),
            z = Mathf.FloorToInt(pos.z / (float)world.GetChunkSize()) * world.GetChunkSize(),
        };
    }
}
