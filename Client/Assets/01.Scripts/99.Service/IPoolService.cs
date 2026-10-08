using UnityEngine;

// 키 단위 오브젝트 풀 관리
public interface IPoolService
{
    void CreatePool(string key, GameObject prefab, int initialSize, Transform parent = null);
    GameObject GetObject(string key);
    bool TryGetObject(string key, out GameObject obj);
    void ReleaseObject(string key, GameObject obj);
    void ReleaseAllObject(string key);
    bool HasPool(string key);
    int GetAvailableCount(string key);
}
