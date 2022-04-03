using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public Tilemap tileMap;

    private void Awake()
    {
        tileMap = GetComponent<Tilemap>();
    }
    
    public void RemoveTile(Vector2 _point)
    {
        Vector3Int _cellPosition = tileMap.WorldToCell(_point);
        tileMap.SetTile(_cellPosition, null);
    }

    
}
