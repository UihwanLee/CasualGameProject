using System.Collections.Generic;
using UnityEngine;

// 프리팹 하나를 재활용하는 오브젝트 풀
// 컴포넌트 타입으로 담아두므로 꺼낼 때 GetComponent가 필요 없다.
// parent를 지정하여 특정 부모 Transform에서 생성할 수 있도록 설계
public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();                // 재활용 오브젝트에 담을 Queue
    private readonly HashSet<T> activeObjects = new HashSet<T>();   // 사용중인 오브젝트
    private readonly T prefab;                                      // 복사하여 사용할 원본 오브젝트
    private readonly Transform parent;                              // 재활용할 오브젝트를 모아둘 부모 Transform

    /// <summary>
    /// ObjectPool 생성
    /// </summary>
    /// <param name="prefab">Pool에 생성할 오브젝트</param>
    /// <param name="initialSize">미리 생성할 개수</param>
    /// <param name="parent">생성 Transform</param>
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Pool에서 오브젝트 가져오기 (없으면 새로 생성)
    /// </summary>
    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab, parent);

        obj.gameObject.SetActive(true);
        activeObjects.Add(obj);

        return obj;
    }

    /// <summary>
    /// Pool에 오브젝트 반환 (이미 반환된 오브젝트는 무시)
    /// </summary>
    public void Release(T obj)
    {
        // 같은 오브젝트를 두 번 넣으면 이후 두 곳에서 동시에 꺼내 쓰게 되므로 막는다
        if (!activeObjects.Remove(obj))
        {
            Debug.LogWarning($"이미 반환되었거나 이 풀의 오브젝트가 아닙니다. obj={obj.name}");
            return;
        }

        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
