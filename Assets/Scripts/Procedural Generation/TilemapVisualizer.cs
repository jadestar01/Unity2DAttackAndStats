using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] private Tilemap floorTilemap;  //칠할 타일맵
    [SerializeField] private TileBase floorTile;    //칠할 바닥 타일
    [Space]
    [Header("Wall")]
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase wallTop;      //칠할 벽 타일
    
    //경로를 받아옴으로써, 타일을 칠하는 함수
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        PaintTiles(floorPosition, floorTilemap, floorTile);
    }

    //경로를 대상으로 칠할 타일맵에 타일을 칠하는 함수
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    //벽을 칠하기 위한 함수
    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    //하나의 타일을 칠하기 위한 함수
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    //타일 맵의 모든 바닥과 벽을 지우기 위한 함수
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

}
