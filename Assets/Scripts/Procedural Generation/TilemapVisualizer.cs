using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] [Header("Floor")] private Tilemap floorTilemap;  //칠할 타일맵
    [SerializeField] [Header("Wall")] private Tilemap wallTilemap;
    [SerializeField] [Header("MapSpriteData")] private MapDataSO mapData;
    
    //경로를 받아옴으로써, 타일을 칠하는 함수
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        PaintTiles(floorPosition, floorTilemap, mapData.floor);
    }

    //경로를 대상으로 칠할 타일맵에 타일을 칠하는 함수
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TilesData> tiles)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, GetRandomTile(tiles), position);
        }
    }

    //벽을 칠하기 위한 함수
    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallTop);
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallSideRight);
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallSideLeft);
        }
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallBottom);
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallFull);
        }

        if(tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    //코너를 칠하기 위한 함수
    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallInnerCornerDownLeft);
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallInnerCornerDownRight);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallDiagonalCornerDownLeft);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallDiagonalCornerDownRight);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallDiagonalCornerUpLeft);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallDiagonalCornerUpRight);
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallFull);
        }
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = GetRandomTile(mapData.wallDiagonalCornerUpLeft);
        }
        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
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

    public TileBase GetRandomTile(List<TilesData> tileList)
    {
        if (tileList.Count == 1)
            return tileList[0].tile;

        TileBase tile = null;
        List<TileBase> tiles = new List<TileBase>();
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].rate; j++)
            {
                tiles.Add(tileList[i].tile);
            }
        }
        Debug.Log(tiles.Count);
        tile = tiles[Random.Range(0, tiles.Count)];
        return tile;
    }
}
