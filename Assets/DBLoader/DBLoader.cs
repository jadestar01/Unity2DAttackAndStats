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
        //Addressable에서 Data를 받아와 Byte[] 변수에 할당한다.
        var databaseData = Addressables.LoadAssetAsync<TextAsset>(BGLoaderForRepoCustom.CustomDatabaseGuid).WaitForCompletion().bytes;
        //수동으로 콘텐츠를 로드한다.
        BGRepo.SetDefaultRepoContent(databaseData);
        BGRepo.Load();
        //DB에 접근한 뒤 데이터를 불러와서 쓴다.
        //Resource Loader와 동일하다.
        BGMetaEntity table = BGRepo.I["EquipItem"];            //Table(Sheet) 선택
        BGEntity entity = table.GetEntity(index);                   //행 가져오기
        Debug.Log("ID : " + entity.Get<int>("ID") + ", Name : " + entity.Get<string>("Name"));
        for (int i = 0; i < entity.Get<List<BGEntity>>("Parameters").Count; i++)
        {
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<ScriptableObject>("Parameter"));
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<float>("Value"));
        }
    }
}