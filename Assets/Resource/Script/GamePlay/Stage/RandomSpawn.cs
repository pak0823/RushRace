// RandomSpawn.cs - 코루틴 분산 생성 + LOD 최적화 적용

using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
        StartCoroutine(SpawnGroup(colliderObj, colliderSpawnCount));
        StartCoroutine(SpawnGroup(noncolliderObj, noncolliderSpawnCount));
#endif
    }

    private IEnumerator SpawnGroup(GameObject[] prefabArray, int count)
    {
        int created = 0;
        int attempts = 0;
        int maxAttempts = count * 5;
        Bounds bounds = colliderFloor.bounds;

        // "Object" 자식 찾기 or 자동 생성
        Transform targetParent = transform.Find("Object");
        if (targetParent == null)
        {
            GameObject obj = new GameObject("Object");
            obj.transform.SetParent(this.transform);
            targetParent = obj.transform;
        }

        while (created < count && attempts < maxAttempts)
        {
            attempts++;

            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 rayOrigin = new Vector3(x, bounds.center.y, z);
            Debug.Log(bounds.center.y);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, pathLayer))
            {
                //Debug.Log($"Ray 미히트 at {rayOrigin}");
                continue;
            }

            Debug.Log($"Ray 히트! Spawn at {hit.point} - {hit.collider.name}");

            int index = Random.Range(0, prefabArray.Length);
            Vector3 spawnPos = hit.point;
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            GameObject obj = Instantiate(prefabArray[index], spawnPos, rotation, targetParent);

            if (obj.GetComponent<LODGroup>() == null)
                TryAddLODGroup(obj);

            created++;

            if (created % 20 == 0)
                yield return null;
        }
    }

    private void TryAddLODGroup(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        LODGroup lodGroup = obj.AddComponent<LODGroup>();
        LOD[] lods = new LOD[2];

        lods[0] = new LOD(0.6f, renderers);              // 가까울 때
        lods[1] = new LOD(0.15f, new Renderer[0]);       // 멀면 꺼짐

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();
    }
}
