using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapSpriteData_", menuName = "Map/MapSpriteData")]
public class MapDataSO : ScriptableObject
{
    [SerializeField] [Header("[�ٴ���]")] public List<TilesData> floor;
    [SerializeField] [Header("[ �� ]")] public List<TilesData> wallTop;
    [SerializeField] [Header("[��  ]")] public List<TilesData> wallSideRight;
    [SerializeField] [Header("[  ��]")] public List<TilesData> wallSideLeft;
    [SerializeField] [Header("[ �� ]")] public List<TilesData> wallBottom;
    [SerializeField] [Header("[ �� ]")] public List<TilesData> wallFull;
    [SerializeField] [Header("[ �� ]")] public List<TilesData> wallInnerCornerDownLeft;
    [SerializeField] [Header("[ �� ]")] public List<TilesData> wallInnerCornerDownRight;
    [SerializeField] [Header("[���ϴ�]")] public List<TilesData> wallDiagonalCornerDownRight;
    [SerializeField] [Header("[���ϴ�]")] public List<TilesData> wallDiagonalCornerDownLeft;
    [SerializeField] [Header("[����]")] public List<TilesData> wallDiagonalCornerUpRight;
    [SerializeField] [Header("[�»��]")] public List<TilesData> wallDiagonalCornerUpLeft;
}

[Serializable]
public struct TilesData
{
    public int rate;
    [PreviewField] public TileBase tile;
}
