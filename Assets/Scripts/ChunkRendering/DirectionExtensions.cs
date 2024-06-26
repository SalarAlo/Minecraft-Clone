using UnityEngine;

public static class DirectionExtensions 
{
    public static Vector3Int GetVector(this Direction direction) {
        return direction switch {
            Direction.up => Vector3Int.up,
            Direction.down => Vector3Int.down,
            Direction.right => Vector3Int.right,
            Direction.left => Vector3Int.left,
            Direction.forward => Vector3Int.forward,
            Direction.back => Vector3Int.back,
            _ => throw new System.Exception()
        };
    }
    
}
