using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static float textureOffset  = .0001f;
    public static float tileSizeX, tileSizeY;
    public static Dictionary<BlockType, TextureData> blockTextureDataDict = new Dictionary<BlockType, TextureData>();
    public BlockDataSO blockDataSO;

    private void Awake() {
        foreach(var textureData in blockDataSO.textureDataList){
            if(!blockTextureDataDict.ContainsKey(textureData.blockType)){
                blockTextureDataDict.Add(textureData.blockType, textureData);
            }
        }
        tileSizeX = blockDataSO.textureSizeX;
        tileSizeY = blockDataSO.textureSizeY;
    }
}
