using BansheeGz.BGDatabase;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Modules.UnityMathematics.Editor;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
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

    private List<BoundsInt> roomsList = new List<BoundsInt>();
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
        roomsList = new();
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
        roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
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
        List<ObjectData> obj = new List<ObjectData>(); obj = Objects.ToList();
        List<ObjectData> objects = new List<ObjectData>();

        //corner wall center normal 순 정렬
        objects = ObjectListSorting(obj);
        
        //type별로 포지션 구하고, 둘 수 있나 확인 후, 두었다면 자리 리셋
        for (int i = 0; i < objects.Count; i++)
        {
            int quantity = Random.Range(objects[i].minQuantity, objects[i].maxQuantity + 1);

            for (int j = 0; j < quantity; j++)
            {
                List<Vector2Int> canPlacePosition = new List<Vector2Int>();
                Vector2Int size = Vector2Int.zero;
                Vector2 centerConsiderPosition = Vector2.zero;

                if (objects[i].objectType == ObjectType.Corner)
                {
                    canPlacePosition = CanPlaceList(room, CornerFinder(room, objects[i]), objects[i].size);
                    size = objects[i].size;
                }
                else if (objects[i].objectType == ObjectType.Center)
                {
                    centerConsiderPosition = CenterPivotFinder(CenterFinder(room), objects[i]);
                    size = CenterConsiderSize(room, CenterFinder(room), objects[i]);
                    canPlacePosition = CanPlaceList(room, CenterPositionList(room, new Vector2Int((int)centerConsiderPosition.x, (int)centerConsiderPosition.y), size), size);
                }
                else if (objects[i].objectType == ObjectType.Wall)
                {
                    canPlacePosition = CanPlaceList(room, WallFinder(room, objects[i]), objects[i].size);
                    size = objects[i].size;
                }
                else if (objects[i].objectType == ObjectType.Normal)
                {
                    canPlacePosition = CanPlaceList(room, room, objects[i].size);
                    size = objects[i].size;
                }

                //설치 할 수 있다면, 설치하고 위치를 삭제한다.
                if (canPlacePosition.Count > 0)
                {
                    Vector2 position;
                    if (objects[i].objectType == ObjectType.Center)
                        position = centerConsiderPosition;
                    else
                        position = canPlacePosition[Random.Range(0, canPlacePosition.Count)];

                    GameObject entity = 
                    Instantiate(objects[i].mapObject,
                                new Vector2(position.x + 0.5f * objects[i].size.x,
                                            position.y + 0.5f * objects[i].size.y),
                                Quaternion.identity);
                    objectList.Add(entity);

                    RemovePosition(room, new Vector2Int((int)position.x, (int)position.y), size);
                }
            }
        }
    }

    private List<ObjectData> ObjectListSorting(List<ObjectData> obj)
    {
        List<ObjectData> objectData = new List<ObjectData>();
        
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i].objectType == ObjectType.Corner)
                objectData.Add(obj[i]);
        }
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i].objectType == ObjectType.Center)
                objectData.Add(obj[i]);
        }
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i].objectType == ObjectType.Wall)
                objectData.Add(obj[i]);
        }
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i].objectType == ObjectType.Normal)
                objectData.Add(obj[i]);
        }

        return objectData;
    }

    //오브젝트를 둘 수 있는 위치 파악
    private List<Vector2Int> CanPlaceList(List<Vector2Int> room, List<Vector2Int> positionList, Vector2Int size)
    {
        List<Vector2Int> canPlacePosition = new List<Vector2Int>();

        for (int i = 0; i < positionList.Count; i++)
        {
            bool canPlace = true;
            //x값 확인
            for (int j = 0; j < size.x; j++)
            {
                //y값 확인
                for (int k = 0; k < size.y; k++)
                {
                    Vector2Int position = new Vector2Int(positionList[i].x + j, positionList[i].y + k);
                    if (!room.Contains(position))
                        canPlace = false;
                }
            }

            if (canPlace)
                canPlacePosition.Add(positionList[i]);
        }

        return canPlacePosition;
    }

    //오브젝트의 크기만큼 방에서 지운다.
    private void RemovePosition(List<Vector2Int> room, Vector2Int position, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector2Int removePosition = new Vector2Int(position.x + i, position.y + j);
                room.Remove(removePosition);
                Debug.Log(removePosition);
            }
        }
    }

    //오브젝트의 크기를 고려한 방의 코너 값을 찾아 반환한다.
    private List<Vector2Int> CornerFinder(List<Vector2Int> room, ObjectData objectData)
    {
        List<Vector2Int> corner = new List<Vector2Int>();
        Vector2Int min = MinPositionFinder(room);
        Vector2Int max = MaxPositionFinder(room);

        corner.Add(min);
        corner.Add(new Vector2Int(min.x, max.y + 1 - objectData.size.y));
        corner.Add(new Vector2Int(max.x + 1 - objectData.size.x, min.y));
        corner.Add(new Vector2Int(max.x + 1 - objectData.size.x, max.y + 1 - objectData.size.y));

        return corner;
    }

    //버그인진 모르겠지만, 만약 설치되어 있는 공간이 있다면, 무시하고 민/맥을 갱신하네, 나쁘진 않은듯.
    private List<Vector2Int> WallFinder(List<Vector2Int> room, ObjectData objectData)
    {
        List<Vector2Int> wall = new List<Vector2Int>();
        Vector2Int min = MinPositionFinder(room);
        Vector2Int max = MaxPositionFinder(room);
        Vector2Int offsetMax = new Vector2Int(max.x + 1 - objectData.size.x, max.y + 1 - objectData.size.y);

        for (int i = 0; i < room.Count; i++)
        {
            if ((room[i].x == min.x || room[i].x == offsetMax.x) && room[i].y <= offsetMax.y)
                wall.Add(room[i]);
            else if (room[i].x <= offsetMax.x && (room[i].y == min.y || room[i].y == offsetMax.y))
                wall.Add(room[i]);
        }

        return wall;
    }

    private Vector2 CenterFinder(List<Vector2Int> room)
    {
        Vector2Int min = MinPositionFinder(room);
        Vector2Int max = MaxPositionFinder(room);
        Vector2 center = new Vector2(((float)(min.x + max.x) / 2 + 0.5f), ((float)(min.y + max.y) / 2 + 0.5f));

        return center;
    }

    private Vector2 CenterPivotFinder(Vector2 center, ObjectData objectData)
    {
        Vector2 pivot = new Vector2(
            (center.x - ((float)(objectData.size.x)/2)),
            (center.y - ((float)(objectData.size.y)/2))
            );

        return pivot;
    }


    private Vector2Int CenterConsiderSize(List<Vector2Int> room, Vector2 center, ObjectData objectData)
    {
        Vector2 pivot = CenterPivotFinder(center, objectData);

        int intX = (int)pivot.x;
        int intY = (int)pivot.y;

        int iteratorX = objectData.size.x;
        int iteratorY = objectData.size.y;

        if (!Mathf.Approximately(intX, pivot.x))
            iteratorX++;
        if (!Mathf.Approximately(intY, pivot.y))
            iteratorY++;

        return new Vector2Int(iteratorX, iteratorY);
    }

    private List<Vector2Int> CenterPositionList(List<Vector2Int> room, Vector2Int centerPivot, Vector2Int size)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                positions.Add(new Vector2Int(centerPivot.x + i, centerPivot.y + j));
            }
        }

        return positions;
    }

    private Vector2Int MinPositionFinder(List<Vector2Int> room)
    {
        int minX = 9999999;
        int minY = 9999999;

        for (int i = 0; i < room.Count; i++)
        {
            if (room[i].x < minX)
                minX = room[i].x;
            if (room[i].y < minY)
                minY = room[i].y;
        }

        return new Vector2Int(minX, minY);
    }

    private Vector2Int MaxPositionFinder(List<Vector2Int> room)
    {
        int maxX = -999999;
        int maxY = -999999;


        for (int i = 0; i < room.Count; i++)
        {
            if (room[i].x > maxX)
                maxX = room[i].x;
            if (room[i].y > maxY)
                maxY = room[i].y;
        }

        return new Vector2Int(maxX, maxY);
    }
}