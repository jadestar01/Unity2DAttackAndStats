using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapSpriteData_", menuName = "Map/MapSpriteData")]
public class MapDataSO : ScriptableObject
{
    [SerializeField] [Header("[¹Ù´ÚÀç]")] public List<TilesData> floor;
    [SerializeField] [Header("[ ¡à ]")] public List<TilesData> wallTop;
    [SerializeField] [Header("[¦¢  ]")] public List<TilesData> wallSideRight;
    [SerializeField] [Header("[  ¦¢]")] public List<TilesData> wallSideLeft;
    [SerializeField] [Header("[ ¡á ]")] public List<TilesData> wallBottom;
    [SerializeField] [Header("[ ¢Ë ]")] public List<TilesData> wallFull;
    [SerializeField] [Header("[ ¦¤ ]")] public List<TilesData> wallInnerCornerDownLeft;
    [SerializeField] [Header("[ ¦£ ]")] public List<TilesData> wallInnerCornerDownRight;
    [SerializeField] [Header("[¿ìÇÏ´Ü]")] public List<TilesData> wallDiagonalCornerDownRight;
    [SerializeField] [Header("[ÁÂÇÏ´Ü]")] public List<TilesData> wallDiagonalCornerDownLeft;
    [SerializeField] [Header("[¿ì»ó´Ü]")] public List<TilesData> wallDiagonalCornerUpRight;
    [SerializeField] [Header("[ÁÂ»ó´Ü]")] public List<TilesData> wallDiagonalCornerUpLeft;
}

[Serializable]
public struct TilesData
{
    public int rate;
    [PreviewField] public TileBase tile;
}
