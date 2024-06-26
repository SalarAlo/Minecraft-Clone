using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockHelper
{
    private static Direction[] directions =
    {
        Direction.back,
        Direction.down,
        Direction.forward,
        Direction.left,
        Direction.right,
        Direction.up
    };

    public static MeshData GetMeshData
        (ChunkData chunk, Vector3Int pos, MeshData meshData, BlockType blockType)
    {

        if (blockType == BlockType.Air || blockType == BlockType.Nothing)
            return meshData;

        foreach (Direction direction in directions)
        {
            var neighbourBlockCoordinates = pos + direction.GetVector();
            var neighbourBlockType = Chunk.GetBlock(chunk, neighbourBlockCoordinates);

            if (neighbourBlockType != BlockType.Nothing && BlockDataManager.blockTextureDataDict[neighbourBlockType].isSolid == false)
            {

                if (blockType == BlockType.Water)
                {
                    if (neighbourBlockType == BlockType.Air)
                        meshData.waterMesh = GetFaceDataIn(direction, chunk, pos, meshData.waterMesh, blockType);
                }
                else
                {
                    meshData = GetFaceDataIn(direction, chunk, pos, meshData, blockType);
                }

            }
        }

        return meshData;
    }

    public static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, Vector3Int pos, MeshData meshData, BlockType blockType)
    {
        GetFaceVertices(direction, pos, meshData, blockType);
        meshData.AddQuadTriangles(BlockDataManager.blockTextureDataDict[blockType].generatesCollider);
        meshData.uv.AddRange(FaceUVs(direction, blockType));


        return meshData;
    }

    public static void GetFaceVertices(Direction direction, Vector3Int pos, MeshData meshData, BlockType blockType)
    {
        int x = pos.x;
        int y = pos.y;
        int z = pos.z;

        var generatesCollider = BlockDataManager.blockTextureDataDict[blockType].generatesCollider;
        //order of vertices matters for the normals and how we render the mesh
        switch (direction)
        {
            case Direction.back:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.forward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;

            case Direction.right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                break;
            default:
                break;
        }
    }

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] UVs = new Vector2[4];
        var tilePos = TexturePosition(direction, blockType);

        UVs[0] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);

        UVs[1] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[2] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[3] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);

        return UVs;
    }

    public static Vector2Int TexturePosition(Direction direction, BlockType blockType)
    {
        return direction switch
        {
            Direction.up => BlockDataManager.blockTextureDataDict[blockType].up,
            Direction.down => BlockDataManager.blockTextureDataDict[blockType].down,
            _ => BlockDataManager.blockTextureDataDict[blockType].side
        };
    }
}