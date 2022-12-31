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
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);           //�ٷ� �� �����͸� �����, �� ���� floorPosition�� �����Ѵ�.
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);              //floorPosition�� �����Ͽ�, ���� �����Ѵ�.
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);   //���� �����Ѵ�.
    }

    //SimpleRandomWalk�� ������ġ�� length�� ������, Iterations��ŭ �ݺ���Ų��.
    //�� SimpleRandomWalk�� �ϳ��� ���� �̾Ƴ��� �˰������̰�, �̸� ������ �۵���Ű�� ���̴�.
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