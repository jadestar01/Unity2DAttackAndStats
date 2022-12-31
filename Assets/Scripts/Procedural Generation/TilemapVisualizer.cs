using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] private Tilemap floorTilemap;  //ĥ�� Ÿ�ϸ�
    [SerializeField] private TileBase floorTile;    //ĥ�� �ٴ� Ÿ��
    [Space]
    [Header("Wall")]
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase wallTop;      //ĥ�� �� Ÿ��
    
    //��θ� �޾ƿ����ν�, Ÿ���� ĥ�ϴ� �Լ�
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        PaintTiles(floorPosition, floorTilemap, floorTile);
    }

    //��θ� ������� ĥ�� Ÿ�ϸʿ� Ÿ���� ĥ�ϴ� �Լ�
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    //���� ĥ�ϱ� ���� �Լ�
    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
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

}
