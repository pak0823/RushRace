using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Prefab 설정")]
    public GameObject[] colliderObj;
    public GameObject[] noncolliderObj;

    [Header("Spawn 갯수 설정")]
    public int colliderSpawnCount;
    public int noncolliderSpawnCount;

    [Header("경로 감지")]
    public LayerMask pathLayer; // 길에만 지정한 Layer

    public Collider colliderFloor;

    private void Awake()
    {
        colliderFloor = GetComponent<Collider>();
        if (colliderFloor == null)
        {
            Debug.LogError("Collider가 없습니다!");
            return;
        }
    }

    public void GenerateAll()
    {
#if UNITY_EDITOR
        SpawnGroup(colliderObj, colliderSpawnCount);
        SpawnGroup(noncolliderObj, noncolliderSpawnCount);
#endif
    }

    public void SpawnGroup(GameObject[] prefabArray, int count)
    {
#if UNITY_EDITOR
        int created = 0;
        int attempts = 0;
        int maxAttempts = count * 5;
        Bounds bounds = colliderFloor.bounds;
        float y = bounds.center.y;
        

        // "Object" 자식 찾기 or 자동 생성
        //Transform targetParent = transform.Find("Object");
        Transform targetParent = transform.Find("Item");
        if (targetParent == null)
        {
            //GameObject obj = new GameObject("Object");
            GameObject obj = new GameObject("Item");
            obj.transform.SetParent(this.transform);
            targetParent = obj.transform;
        }

        while (created < count && attempts < maxAttempts)
        {
            attempts++;

            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 checkPos = new Vector3(x, y, z);

            float checkRadius = 1f;
            bool overlapsPath = Physics.CheckSphere(checkPos, checkRadius, pathLayer);

            //if (overlapsPath)
            //    continue;
            Debug.Log("CheckSphere 감지 결과: " + Physics.CheckSphere(checkPos, 1f, pathLayer));
            if (!overlapsPath)
            {
                continue;
            }

            int index = Random.Range(0, prefabArray.Length);
            Vector3 spawnPos = new Vector3(x, bounds.center.y, z);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            Instantiate(prefabArray[index], spawnPos, rotation, targetParent);
            created++;
        }
        Debug.Log("생성된 총 갯수: " +created);
#endif
    }

    
}
