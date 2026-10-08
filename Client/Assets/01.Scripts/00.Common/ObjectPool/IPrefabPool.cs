using UnityEngine;

// 같은 컴포넌트 타입의 프리팹 여러 개를 키별로 풀링
public interface IPrefabPool<T> where T : Component
{
    void Register(string key, T prefab, int initialSize);
    T Spawn(string key);
    void Despawn(T obj);
}
