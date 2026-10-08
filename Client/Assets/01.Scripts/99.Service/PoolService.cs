using System.Collections.Generic;
using UnityEngine;

// ObjectPool을 모아놓은 PoolService
// ObjectPool Dictionary 자료구조로 관리
// 풀 오브젝트는 생성자로 받은 root 아래에 키별 부모를 만들어 모아둔다.
public class PoolService : IPoolService
{
    private readonly Transform root;
    private readonly Dictionary<string, ObjectPool> objectPools = new Dictionary<string, ObjectPool>();

    public PoolService(Transform root)
    {
        this.root = root;
    }

    /// <summary>
    /// ObjectPool 생성
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <param name="prefab">Pool 오브젝트</param>
    /// <param name="initialSize">오브젝트 생성 개수</param>
    /// <param name="parent">오브젝트 생성 부모</param>
    public void CreatePool(string key, GameObject prefab, int initialSize, Transform parent = null)
    {
        if (objectPools.ContainsKey(key))
        {
            Debug.Log($"해당 {key}값을 가지고 있는 ObjectPool이 이미 존재합니다.");
            return;
        }

        Transform newParent = parent;
        if (newParent == null)
        {
            GameObject poolParent = new GameObject($"Pool_{key}");
            poolParent.transform.SetParent(root);
            newParent = poolParent.transform;
        }

        ObjectPool pool = new ObjectPool(prefab, initialSize, newParent);
        objectPools.Add(key, pool);
    }

    /// <summary>
    /// ObjectPool에서 Object 가져오기
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <returns>Pool Object</returns>
    public GameObject GetObject(string key)
    {
        // Dictionary에서 해당 key에 맞는 object를 가져옴
        if (objectPools.TryGetValue(key, out ObjectPool pool))
        {
            return pool.Get();
        }

        Debug.Log($"해당 {key}값을 가지고 있는 ObjectPool이 존재하지 않습니다");
        return null;
    }

    public bool TryGetObject(string key, out GameObject obj)
    {
        obj = null;

        if (objectPools.TryGetValue(key, out ObjectPool pool))
            return pool.TryGet(out obj);

        Debug.Log($"해당 {key}값을 가지고 있는 ObjectPool이 존재하지 않습니다");
        return false;
    }

    /// <summary>
    /// ObjectPool에 Object 반납
    /// </summary>
    /// <param name="key">Pool Key</param>
    /// <param name="obj">반납할 Object</param>
    public void ReleaseObject(string key, GameObject obj)
    {
        // Dicitonay에서 해당 key에 맞는 Pool 가져옴
        if (objectPools.TryGetValue(key, out ObjectPool pool))
        {
            pool.Release(obj);
        }
        else
        {
            // Pool 없다면 오브젝트를 강제로 비활성화하여 디버그 출력
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
            }

            Debug.Log($"해당 {key}값을 가지고 있는 ObjectPool이 존재하지 않습니다");
        }
    }

    /// <summary>
    /// 해당 Key에 있는 모든 Object 반납
    /// </summary>
    /// <param name="key">Pool key</param>
    public void ReleaseAllObject(string key)
    {
        // Dicitonay에서 해당 key에 맞는 Pool 가져옴
        if (objectPools.TryGetValue(key, out ObjectPool pool))
        {
            pool.ReleaseAll();
        }
    }

    public bool HasPool(string key)
    {
        return objectPools.ContainsKey(key);
    }

    public int GetAvailableCount(string key)
    {
        if (objectPools.TryGetValue(key, out ObjectPool pool))
            return pool.AvailableCount;

        return 0;
    }
}
