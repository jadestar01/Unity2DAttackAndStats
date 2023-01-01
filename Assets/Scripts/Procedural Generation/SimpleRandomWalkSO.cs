using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGenerateData_",menuName = "Map/MapGenerateData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    //SimpleRandomWalk�� �ݺ��� Ƚ��, �� ���� �����̴�.
    //���� �����̴�. ���� �����¿�� �̵��Ѵ�. 
    public int iterations = 10, walkLength = 10;
    //���� �̵���ġ�� �����ִ� ���̴�. true��� �����ϰ� �ǰ�, false��� ������������ ���Ƿ�, �ִ� 2length*2length�� ���簢���� ���� �� �ִ�.
    public bool startRandomlyEachIteration = true;
}
