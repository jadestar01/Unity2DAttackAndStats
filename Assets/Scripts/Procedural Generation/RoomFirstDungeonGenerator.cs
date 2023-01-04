using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int minRoomCount = 10, maxRoomCount = 10;
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;           //방의 최소 사이즈
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;         //던전의 총 크기
    [SerializeField] [Range(0, 10)] private int offset = 1;                     //방 사이의 거리  0이라면, 방끼리 붙게 된다.
    //[SerializeField] private bool randomWalkRooms = false;
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary
    = new Dictionary<Vector2Int, HashSet<Vector2Int>>();                        //방의 데이터    Center, 방의 타일
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();     //바닥의 데이터
    private HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();  //복도의 데이터
    [SerializeField] GameObject textPanel;
    [SerializeField] GameObject text;
    [SerializeField] private GameObject panel;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        //이진 공간 분할법으로 공간을 만들고, floor에 해당 내용을 넣는다.
        bool RoomSizeDetecter = false;
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        while (!RoomSizeDetecter)
        {
            if (roomsList.Count >= minRoomCount && roomsList.Count <= maxRoomCount)
                RoomSizeDetecter = true;
            else
                roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        }
        NumberToCenter(roomsList);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        /*
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        */
        floor = CreateSimpleRooms(roomsList);

        //방의 중앙을 찾아내서, 방을 연결시키고, floor에 해당 내용(복도)을 넣는다.
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corriors = ConnectRooms(roomCenters);
        floor.UnionWith(corriors);

        //칠하고, 벽을 세운다.
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    public void NumberToCenter(List<BoundsInt> roomsList)
    {
        Debug.Log(roomsList.Count);
        if(panel != null)
            DestroyImmediate(panel);
        panel = Instantiate(textPanel);
        for (int i = 0; i < roomsList.Count; i++)
        {
            Debug.Log(i + " : " + roomsList[i].center);
            GameObject item = Instantiate(text, roomsList[i].center, Quaternion.identity);
            item.name = i.ToString();
            item.transform.SetParent(panel.transform);
            text.GetComponent<TMP_Text>().text = item.name;
        }
        for(int i = 0; i < roomsList.Count; i++)
            Debug.Log(i + " : " + roomsList[i].center);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset)
                && position.x <= (roomBounds.xMax - offset)
                && position.y >= (roomBounds.yMin - offset)
                && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private void ClearRoomData()
    {
        roomsDictionary.Clear();
    }

    private void SaveRoomData(Vector2Int roomCenter, HashSet<Vector2Int> roomBound)
    {
        roomsDictionary[roomCenter] = roomBound;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        //Y추적
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        //X추적
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        //collidorPositions에 해당 corridor를 집어넣는다.
        corridorPositions.AddRange(corridor);
        return corridor;
    }

    //방의 중심을 재서, 현재 방에서 가장 가까운 방을 찾아 리턴한다. 
    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> eachRoom = new HashSet<Vector2Int>();
        ClearRoomData();
        foreach (var room in roomList)
        {
            eachRoom.Clear();
            for (int col = offset;  col < room.size.x - offset;  col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                    eachRoom.Add(position);
                }
            }
            SaveRoomData((Vector2Int)Vector3Int.RoundToInt(room.center), eachRoom);
        }
        //floorPositions에 해당 floor를 집어넣는다.
        floorPositions.AddRange(floor);
        return floor;
    }
}
