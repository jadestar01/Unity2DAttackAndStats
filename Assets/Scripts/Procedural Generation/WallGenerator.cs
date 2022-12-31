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
        foreach (var position in floorPosition)                             //모든 바닥위치를 읽어서,
        {
            foreach (var direction in directionList)                        //해당 위치에 상하좌우를 담는데,
            {
                var neightbourPosition = position + direction;              
                if(floorPosition.Contains(neightbourPosition) == false)     //그 상하좌우의 위치가 바닥위치가 아니라면,
                    wallPostions.Add(neightbourPosition);                    //벽 위치에 그 값을 넣는다.
            }
        }
        return wallPostions;
    }
}
