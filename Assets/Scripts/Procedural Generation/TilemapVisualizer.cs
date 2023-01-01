using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] [Header("Floor")] private Tilemap floorTilemap;  //ĥ�� Ÿ�ϸ�
    [SerializeField] [Header("Wall")] private Tilemap wallTilemap;
    [SerializeField] [Header("MapSpriteData")] private MapDataSO mapData;
    
    //��θ� �޾ƿ����ν�, Ÿ���� ĥ�ϴ� �Լ�
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        PaintTiles(floorPosition, floorTilemap, mapData.floor);
    }

    //��θ� ������� ĥ�� Ÿ�ϸʿ� Ÿ���� ĥ�ϴ� �Լ�
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TilesData> tiles)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, GetRandomTile(tiles), position);
        }
    }

    //���� ĥ�ϱ� ���� �Լ�
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

    //�ڳʸ� ĥ�ϱ� ���� �Լ�
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

    //�ϳ��� Ÿ���� ĥ�ϱ� ���� �Լ�
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    //Ÿ�� ���� ��� �ٴڰ� ���� ����� ���� �Լ�
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
