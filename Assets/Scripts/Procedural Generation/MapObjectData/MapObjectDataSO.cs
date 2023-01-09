using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MapObjectData_", menuName = "Map/MapObjectData")]
public class MapObjectDataSO : ScriptableObject
{
    public List<ObjectData> normal = new List<ObjectData>();
    public List<ObjectData> start = new List<ObjectData>();
    public List<ObjectData> boss = new List<ObjectData>();
    public List<ObjectData> exit = new List<ObjectData>();
}

[Serializable]
public class ObjectData
{
    [Space][Header("ObjectData")]
    public GameObject mapObject;
    public Vector2Int size = Vector2Int.one;

    [Space, Header("Placemnet Type : ")]
    public ObjectType objectType;
    //[Min(1)]
    public int minQuantity = 1;
    //[Min(1)]
    public int maxQuantity = 1;
}

public enum ObjectType
{
    Normal,
    Corner,
    Center,
    Wall
}