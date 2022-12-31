using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPosition, Direction2D.cardinalDirectionsList);
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPostions = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)                             //��� �ٴ���ġ�� �о,
        {
            foreach (var direction in directionList)                        //�ش� ��ġ�� �����¿츦 ��µ�,
            {
                var neightbourPosition = position + direction;              
                if(floorPosition.Contains(neightbourPosition) == false)     //�� �����¿��� ��ġ�� �ٴ���ġ�� �ƴ϶��,
                    wallPostions.Add(neightbourPosition);                    //�� ��ġ�� �� ���� �ִ´�.
            }
        }
        return wallPostions;
    }
}
