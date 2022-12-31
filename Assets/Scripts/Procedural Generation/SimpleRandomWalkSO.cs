using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_",menuName = "Map/MapData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    //SimpleRandomWalk를 반복할 횟수, 즉 줄의 개수이다.
    //줄의 길이이다. 줄은 상하좌우로 이동한다. 
    public int iterations = 10, walkLength = 10;
    //줄의 이동위치를 정해주는 것이다. true라면 랜덤하게 되고, false라면 시작점에서만 가므로, 최대 2length*2length의 정사각형을 얻을 수 있다.
    public bool startRandomlyEachIteration = true;
}
