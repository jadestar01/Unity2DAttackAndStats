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
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;           //���� �ּ� ������
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;         //������ �� ũ��
    [SerializeField] [Range(0, 10)] private int offset = 1;                     //�� ������ �Ÿ�  0�̶��, �波�� �ٰ� �ȴ�.
    [SerializeField] private bool seeNumber = false;
    [SerializeField] private MapObjectDataSO objectData;
    //[SerializeField] private bool randomWalkRooms = false;
    private Dictionary<Vector2Int, List<Vector2Int>> roomsDictionary
    = new Dictionary<Vector2Int, List<Vector2Int>>();                        //���� ������    Center, ���� Ÿ��
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();     //�ٴ��� ������
    private HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();  //������ ������

    private Dictionary<Vector2Int, int> roomNumber = new Dictionary<Vector2Int, int>();     //���� �ѹ� 
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

        //���� ���� ���ҹ����� ������ �����, floor�� �ش� ������ �ִ´�.
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
        //���� �����Ѵ�.
        floor = CreateSimpleRooms(roomsList);

        //���� �߾��� ã�Ƴ���, ���� �����Ű��, floor�� �ش� ����(����)�� �ִ´�.
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        //roomCenters�� �����Ѵ�.
        List<Vector2Int> roomCenterList = new List<Vector2Int>();
        roomCenterList = roomCenters.ToList();
        
        //���� �����ϰ�, ������ �����.
        HashSet<Vector2Int> corriors = ConnectRooms(roomCenters);
        floor.UnionWith(corriors);

        //���� Ư���� �м��Ͽ� ������, Ż���, ���۹��� ���Ѵ�.
        RoomSelector(roomCenterList);

        //�濡 ��ȣ�� �ű��.
        if (seeNumber)
            NumberToCenter(roomsList);

        //ĥ�ϰ�, ���� �����.
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
        //Y����
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
        //X����
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
        //collidorPositions�� �ش� corridor�� ����ִ´�.
        corridorPositions.AddRange(corridor);
        return corridor;
    }

    //���� �߽��� �缭, ���� �濡�� ���� ����� ���� ã�� �����Ѵ�. 
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
        //floorPositions�� �ش� floor�� ����ִ´�.
        floorPositions.AddRange(floor);
        return floor;
    }

    private void RoomSelector(List<Vector2Int> roomCenters)
    {
        Vector2Int farthest = Vector2Int.zero;
        float distance = 0;
        //Start��
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

        //Boss��
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

        //���� �� �ִ°� �ľ��ϱ�, ���� �� �����ϱ�, 

        //Corner Object �α�

        //Center Object �α�

        //Wall Object �α�

        //Normal Object �α�
        for (int i = 0; i < objects.Count; i++)     //Normal ������Ʈ�� ����
        {
            int quantity = Random.Range(objects[i].minQuantity, objects[i].maxQuantity + 1);

            for (int j = 0; j < quantity; j++)
            {
                List<Vector2Int> canPlacePosition = new List<Vector2Int>();
                canPlacePosition = CanPlaceList(room, objects[i]);      //��ġ�� �� �ִ� ��ġ�� �����Ѵ�!

                //��ġ �� �� �ִٸ�, ��ġ�ϰ� ��ġ�� �����Ѵ�.
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

    //������Ʈ�� �� �� �ִ� ��ġ �ľ�
    private List<Vector2Int> CanPlaceList(List<Vector2Int> room, ObjectData objectData)
    {
        List<Vector2Int> canPlacePosition = new List<Vector2Int>();

        for (int i = 0; i < room.Count; i++)
        {
            bool canPlace = true;
            //x�� Ȯ��
            for (int j = 0; j < objectData.size.x; j++)
            {
                //y�� Ȯ��
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