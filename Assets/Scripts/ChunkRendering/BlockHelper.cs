using UnityEngine;

public static class BlockHelper
{
    private static Direction[] directions = {
        Direction.back,
        Direction.down,
        Direction.forward,
        Direction.left,
        Direction.right,
        Direction.up
    };

    public static void AddBlockMeshData(MeshData meshData, ChunkData chunkData, Vector3Int localBlockPos, BlockType blockType) {
        // dont have to be rendered anyways
        if (blockType == BlockType.Air || blockType == BlockType.Nothing) return;

        foreach (Direction direction in directions) {
            // Get the neighbour based on current direction
            var neighbourBlockCoordinates = localBlockPos + direction.GetVector();
            bool neighbourBlockIsTop = neighbourBlockCoordinates.y == chunkData.worldRef.GetChunkHeight();
            var neighbourBlockType = neighbourBlockIsTop ? BlockType.Air : Chunk.GetBlock(chunkData, neighbourBlockCoordinates);

            bool neighbourBlockIsInWorld = neighbourBlockType != BlockType.Nothing;
            if(!neighbourBlockIsInWorld) continue;

            // becouse if block on current dir is solid it isnt seethrough so no need to add quad on that dir becouse isnt visible anyways 
            bool neighbourBlockIsSolid = BlockDataManager.blockTextureDataDict[neighbourBlockType].isSolid;
            if (neighbourBlockIsSolid) continue;

            bool blockIsWater = blockType == BlockType.Water;
            bool neighbourBlockIsAir = neighbourBlockType == BlockType.Air;

            if (blockIsWater) {
                // only need to render the water if the air is neighbour becouse then its visible to player
                if (neighbourBlockIsAir) AddQuadToMeshData(direction, localBlockPos, meshData.waterMesh, blockType);
            } else {
                // render normal block which sticks out and is visible becouse its neighbour is not solid 
                AddQuadToMeshData(direction, localBlockPos, meshData, blockType);
            }
        }
    }

    public static void AddQuadToMeshData(Direction direction, Vector3Int pos, MeshData meshData, BlockType blockType) {
        AddQuadVertices(direction, pos, meshData, blockType);
        meshData.AddQuadTriangles(BlockDataManager.blockTextureDataDict[blockType].generatesCollider);
        meshData.uv.AddRange(FaceUVs(direction, blockType));
    }

    public static void AddQuadVertices(Direction direction, Vector3Int pos, MeshData meshData, BlockType blockType) {
        int x = pos.x;
        int y = pos.y;
        int z = pos.z;

        var generatesCollider = BlockDataManager.blockTextureDataDict[blockType].generatesCollider;
        //order of vertices matters for the normals and how we render the mesh
        switch (direction) {
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

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType) {
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

    public static Vector2Int TexturePosition(Direction direction, BlockType blockType) {
        return direction switch
        {
            Direction.up => BlockDataManager.blockTextureDataDict[blockType].up,
            Direction.down => BlockDataManager.blockTextureDataDict[blockType].down,
            _ => BlockDataManager.blockTextureDataDict[blockType].side
        };
    }
}