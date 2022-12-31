using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkSO randomWalkParameters;


    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);           //줄로 맵 데이터를 만들고, 그 값을 floorPosition에 저장한다.
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);              //floorPosition을 전달하여, 맵을 생성한다.
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);   //벽을 생성한다.
    }

    //SimpleRandomWalk에 시작위치와 length를 보내어, Iterations만큼 반복시킨다.
    //즉 SimpleRandomWalk는 하나의 줄을 뽑아내는 알고리즘이고, 이를 여러번 작동시키는 것이다.
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO randomWalkParameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);
            if (randomWalkParameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
