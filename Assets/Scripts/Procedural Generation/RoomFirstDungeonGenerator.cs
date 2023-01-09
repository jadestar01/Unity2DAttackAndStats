using BansheeGz.BGDatabase;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Modules.UnityMathematics.Editor;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int minRoomCount = 10, maxRoomCount = 10;
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;           //방의 최소 사이즈
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;         //던전의 총 크기
    [SerializeField] [Range(0, 10)] private int offset = 1;                     //방 사이의 거리  0이라면, 방끼리 붙게 된다.
    [SerializeField] private bool seeNumber = false;
    [SerializeField] private MapObjectDataSO objectData;
    //[SerializeField] private bool randomWalkRooms = false;
    private Dictionary<Vector2Int, List<Vector2Int>> roomsDictionary
    = new Dictionary<Vector2Int, List<Vector2Int>>();                        //방의 데이터    Center, 방의 타일
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();     //바닥의 데이터
    private HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();  //복도의 데이터

    private Dictionary<Vector2Int, int> roomNumber = new Dictionary<Vector2Int, int>();     //방의 넘버 
    [SerializeField] GameObject textPanel;
    [SerializeField] GameObject text;
    [SerializeField] GameObject axisText;
    [SerializeField] private GameObject panel;

    [Header("Green")] public Vector2Int startRoom = new Vector2Int();
    [Header("Blue")] public Vector2Int exitRoom = new Vector2Int();
    [Header("Red")] public Vector2Int bossRoom = new Vector2Int();

    private List<GameObject> objectList = new List<GameObject>();

    protected override void RunProceduralGeneration()
    {
        ObjectReset();
        CreateRooms();
    }

    private void ObjectReset()
    {
        int count = objectList.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(objectList[count - 1 - i]);
        }
    }

    private void CreateRooms()
    {
        if (panel != null)
            DestroyImmediate(panel);
        if(seeNumber)
            panel = Instantiate(textPanel);

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
        //방을 생성한다.
        floor = CreateSimpleRooms(roomsList);

        //방의 중앙을 찾아내서, 방을 연결시키고, floor에 해당 내용(복도)을 넣는다.
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        //roomCenters를 복제한다.
        List<Vector2Int> roomCenterList = new List<Vector2Int>();
        roomCenterList = roomCenters.ToList();
        
        //방을 연결하고, 복도를 만든다.
        HashSet<Vector2Int> corriors = ConnectRooms(roomCenters);
        floor.UnionWith(corriors);

        //방의 특성을 분석하여 보스방, 탈출방, 시작방을 정한다.
        RoomSelector(roomCenterList);

        //방에 번호를 매긴다.
        if (seeNumber)
            NumberToCenter(roomsList);

        //칠하고, 벽을 세운다.
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        CreateObject();
    }

    public void NumberToCenter(List<BoundsInt> roomsList)
    {
        for (int i = 0; i < roomsList.Count; i++)
        {
            roomNumber.Add((Vector2Int)Vector3Int.RoundToInt(roomsList[i].center), i);
            GameObject item = Instantiate(text, roomsList[i].center, Quaternion.identity);
            item.name = i.ToString();
            item.transform.SetParent(panel.transform);
            item.GetComponent<RoomText>().SetRoomNumber(i);
            if ((Vector2Int)Vector3Int.RoundToInt(roomsList[i].center) == exitRoom)
                item.GetComponent<TMP_Text>().color = Color.blue;
            else if ((Vector2Int)Vector3Int.RoundToInt(roomsList[i].center) == bossRoom)
                item.GetComponent<TMP_Text>().color = Color.red;
            else if ((Vector2Int)Vector3Int.RoundToInt(roomsList[i].center) == startRoom)
                item.GetComponent<TMP_Text>().color = Color.green;
        }
    }
    public void AxisNumToPosition(HashSet<Vector2Int> room)
    {
        foreach (Vector2Int roomdata in room)
        {
            GameObject item = Instantiate(axisText, new Vector2(roomdata.x + 0.5f, roomdata.y + 0.5f), Quaternion.identity);
            item.GetComponent<TMP_Text>().text = roomdata.ToString();
            item.transform.SetParent(panel.transform);
        }
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
        roomNumber.Clear();
    }

    private void SaveRoomData(Vector2Int roomCenter, HashSet<Vector2Int> roomBound)
    {
        List<Vector2Int> room = new List<Vector2Int>();
        foreach (var item in roomBound)
        {
            room.Add(item);
        }
        roomsDictionary.Add(roomCenter, room.ToList());
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
            exitRoom = closest;
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
            for (int row = offset;  row < room.size.y - offset;  row++)
            {
                for (int col = offset; col < room.size.x - offset; col++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                    eachRoom.Add(position);
                }
            }
            SaveRoomData((Vector2Int)Vector3Int.RoundToInt(room.center), eachRoom);
            if (seeNumber)
                AxisNumToPosition(eachRoom);
        }
        //floorPositions에 해당 floor를 집어넣는다.
        floorPositions.AddRange(floor);
        return floor;
    }

    private void RoomSelector(List<Vector2Int> roomCenters)
    {
        Vector2Int farthest = Vector2Int.zero;
        float distance = 0;
        //Start방
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, exitRoom);
            if (currentDistance > distance)
            {
                distance = currentDistance;
                farthest = position;
            }
        }
        startRoom = farthest;
        roomCenters.Remove(startRoom);

        //Boss방
        farthest = Vector2Int.zero;
        distance = 0;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, exitRoom) + Vector2.Distance(position, startRoom);
            if (currentDistance > distance)
            {
                distance = currentDistance;
                farthest = position;
            }
        }
        bossRoom = farthest;
    }

    private void CreateObject()
    {
        foreach (var room in roomsDictionary)
        {
            if (room.Key == startRoom) ObjectSetter(room.Value, objectData.start);
            else if (room.Key == bossRoom) ObjectSetter(room.Value, objectData.boss);
            else if (room.Key == exitRoom) ObjectSetter(room.Value, objectData.exit);
            else ObjectSetter(room.Value, objectData.normal);
        }
    }

    private void ObjectSetter(List<Vector2Int> Room, List<ObjectData> Objects)
    {
        List<Vector2Int> room = new List<Vector2Int>(); room = Room.ToList();
        List<ObjectData> objects = new List<ObjectData>(); objects = Objects.ToList();

        //놓을 수 있는가 파악하기, 놓고 룸 제거하기, 

        //Corner Object 두기

        //Center Object 두기

        //Wall Object 두기

        //Normal Object 두기
        for (int i = 0; i < objects.Count; i++)     //Normal 오브젝트에 접근
        {
            int quantity = Random.Range(objects[i].minQuantity, objects[i].maxQuantity + 1);

            for (int j = 0; j < quantity; j++)
            {
                List<Vector2Int> canPlacePosition = new List<Vector2Int>();
                canPlacePosition = CanPlaceList(room, objects[i]);      //설치할 수 있는 위치를 추적한다!

                //설치 할 수 있다면, 설치하고 위치를 삭제한다.
                if (canPlacePosition.Count > 0)
                {
                    Vector2Int position = canPlacePosition[Random.Range(0, canPlacePosition.Count)];
                    Debug.Log(position);
                    GameObject entity = 
                    Instantiate(objects[i].mapObject,
                                new Vector2(position.x + 0.5f * objects[i].size.x,
                                            position.y + 0.5f * objects[i].size.y),
                                Quaternion.identity);
                    objectList.Add(entity);
                    RemovePosition(room, position, objects[i]);
                }
            }
        }
    }

    //오브젝트를 둘 수 있는 위치 파악
    private List<Vector2Int> CanPlaceList(List<Vector2Int> room, ObjectData objectData)
    {
        List<Vector2Int> canPlacePosition = new List<Vector2Int>();

        for (int i = 0; i < room.Count; i++)
        {
            bool canPlace = true;
            //x값 확인
            for (int j = 0; j < objectData.size.x; j++)
            {
                //y값 확인
                for (int k = 0; k < objectData.size.y; k++)
                {
                    Vector2Int position = new Vector2Int(room[i].x + j, room[i].y + k);
                    if (!room.Contains(position))
                        canPlace = false;
                }
            }

            if (canPlace)
                canPlacePosition.Add(room[i]);
        }

        return canPlacePosition;
    }

    private void RemovePosition(List<Vector2Int> room, Vector2Int position, ObjectData objectData)
    {
        for (int i = 0; i < objectData.size.x; i++)
        {
            for (int j = 0; j < objectData.size.y; j++)
            {
                Vector2Int removePosition = new Vector2Int(position.x + i, position.y + j);
                Debug.Log(removePosition);
                room.Remove(removePosition);
            }
        }
    }
}