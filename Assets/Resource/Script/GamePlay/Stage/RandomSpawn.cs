// RandomSpawn.cs - �ڷ�ƾ �л� ���� + LOD ����ȭ ����

using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RandomSpawn : MonoBehaviour
{
    [Header("Prefab ����")]
    public GameObject[] colliderObj;
    public GameObject[] noncolliderObj;

    [Header("Spawn ���� ����")]
    public int colliderSpawnCount;
    public int noncolliderSpawnCount;

    [Header("��� ����")]
    public LayerMask pathLayer; // �濡�� ������ Layer
    public Collider colliderFloor;

    private void Awake()
    {
        colliderFloor = GetComponent<Collider>();
        if (colliderFloor == null)
        {
            Debug.LogError("Collider�� �����ϴ�!");
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

        // "Object" �ڽ� ã�� or �ڵ� ����
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
                //Debug.Log($"Ray ����Ʈ at {rayOrigin}");
                continue;
            }

            Debug.Log($"Ray ��Ʈ! Spawn at {hit.point} - {hit.collider.name}");

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

        lods[0] = new LOD(0.6f, renderers);              // ����� ��
        lods[1] = new LOD(0.15f, new Renderer[0]);       // �ָ� ����

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();
    }
}
