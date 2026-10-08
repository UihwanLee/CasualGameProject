using System.Collections.Generic;
using UnityEngine;

// 같은 컴포넌트 타입의 프리팹 여러 개를 키별 ObjectPool로 관리하는 풀
// 키는 데이터의 Prefab 값(프리팹 이름)을 그대로 쓰므로 꺼낼 때 문자열을 새로 만들지 않는다.
// 꺼낸 오브젝트가 어느 풀에서 나왔는지 기억해 두므로 반납할 때 키가 필요 없다.
public class PrefabPool<T> : IPrefabPool<T> where T : Component
{
    private readonly Transform root;
    private readonly Dictionary<string, ObjectPool<T>> pools = new Dictionary<string, ObjectPool<T>>();
    private readonly Dictionary<T, ObjectPool<T>> owners = new Dictionary<T, ObjectPool<T>>();  // 꺼낸 오브젝트 → 소속 풀

    public PrefabPool(Transform root)
    {
        this.root = root;
    }

    /// <summary>
    /// 프리팹 등록 후 미리 생성
    /// </summary>
    /// <param name="key">풀 키 (데이터의 Prefab 값)</param>
    /// <param name="prefab">원본 프리팹</param>
    /// <param name="initialSize">미리 생성할 개수</param>
    public void Register(string key, T prefab, int initialSize)
    {
        if (pools.ContainsKey(key))
        {
            Debug.LogError($"이미 등록된 풀 키입니다. key={key}");
            return;
        }

        Transform parent = new GameObject($"Pool_{typeof(T).Name}_{key}").transform;
        parent.SetParent(root);

        pools.Add(key, new ObjectPool<T>(prefab, initialSize, parent));
    }

    /// <summary>
    /// 키에 해당하는 풀에서 오브젝트 꺼내기 (등록되지 않은 키면 null)
    /// </summary>
    public T Spawn(string key)
    {
        if (!pools.TryGetValue(key, out ObjectPool<T> pool))
        {
            Debug.LogError($"등록되지 않은 풀 키입니다. key={key}");
            return null;
        }

        T obj = pool.Get();
        owners[obj] = pool;

        return obj;
    }

    /// <summary>
    /// 꺼냈던 풀로 오브젝트 반납
    /// </summary>
    public void Despawn(T obj)
    {
        if (!owners.Remove(obj, out ObjectPool<T> pool))
        {
            Debug.LogWarning($"이 풀에서 꺼낸 오브젝트가 아닙니다. obj={obj.name}");
            return;
        }

        pool.Release(obj);
    }
}
