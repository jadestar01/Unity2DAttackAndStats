using BansheeGz.BGDatabase;
using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DBLoader : MonoBehaviour
{
    void Awake()
    {

    }

    [Button]
    private void TEST(int index)
    {
        //Addressable���� Data�� �޾ƿ� Byte[] ������ �Ҵ��Ѵ�.
        var databaseData = Addressables.LoadAssetAsync<TextAsset>(BGLoaderForRepoCustom.CustomDatabaseGuid).WaitForCompletion().bytes;
        //�������� �������� �ε��Ѵ�.
        BGRepo.SetDefaultRepoContent(databaseData);
        BGRepo.Load();
        //DB�� ������ �� �����͸� �ҷ��ͼ� ����.
        //Resource Loader�� �����ϴ�.
        BGMetaEntity table = BGRepo.I["EquipItem"];            //Table(Sheet) ����
        BGEntity entity = table.GetEntity(index);                   //�� ��������
        Debug.Log("ID : " + entity.Get<int>("ID") + ", Name : " + entity.Get<string>("Name"));
        for (int i = 0; i < entity.Get<List<BGEntity>>("Parameters").Count; i++)
        {
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<ScriptableObject>("Parameter"));
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<float>("Value"));
        }
    }
}