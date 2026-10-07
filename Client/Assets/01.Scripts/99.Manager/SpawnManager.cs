using System.Collections.Generic;
using UnityEngine;

// 몬스터 생성/반납과 필드 위 몬스터 목록을 관리하는 매니저 (씬마다 하나)
// 몬스터 프리팹은 이름으로 찾으며, MonsterData.Prefab과 프리팹 이름이 같아야 한다.
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("몬스터 경로")]
    [SerializeField] private MonsterPath path;

    [Header("몬스터 프리팹")]
    [SerializeField] private Monster[] monsterPrefabs;
    [SerializeField] private int poolInitialSize = 20;

    private readonly List<Monster> activeMonsters = new List<Monster>();

    public IReadOnlyList<Monster> ActiveMonsters => activeMonsters;
    public int MonsterCount => activeMonsters.Count;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// 몬스터 생성 후 경로 시작 지점에 배치
    /// </summary>
    /// <param name="monsterId">몬스터 ID</param>
    public Monster Spawn(int monsterId)
    {
        MonsterData data = DataManager.GetMonster(monsterId);
        if (data == null)
        {
            Debug.LogError($"MonsterData가 없습니다. id={monsterId}");
            return null;
        }

        string key = Monster.GetPoolKey(monsterId);
        if (!PoolManager.Instance.HasPool(key))
            CreatePool(key, data.Prefab);

        GameObject go = PoolManager.Instance.GetObject(key);
        if (go == null)
            return null;

        Monster monster = go.GetComponent<Monster>();
        monster.Init(data, path);

        activeMonsters.Add(monster);
        EventBus.OnMonsterCountChanged?.Invoke(activeMonsters.Count);

        return monster;
    }

    /// <summary>
    /// 몬스터를 목록에서 빼고 풀에 반납
    /// </summary>
    public void Despawn(Monster monster)
    {
        if (!activeMonsters.Remove(monster))
            return;

        PoolManager.Instance.ReleaseObject(monster.PoolKey, monster.gameObject);
        EventBus.OnMonsterCountChanged?.Invoke(activeMonsters.Count);
    }

    /// <summary>
    /// 필드 위 모든 몬스터 반납
    /// </summary>
    public void DespawnAll()
    {
        for (int i = activeMonsters.Count - 1; i >= 0; i--)
        {
            Monster monster = activeMonsters[i];
            PoolManager.Instance.ReleaseObject(monster.PoolKey, monster.gameObject);
        }

        activeMonsters.Clear();
        EventBus.OnMonsterCountChanged?.Invoke(0);
    }

    private void CreatePool(string key, string prefabName)
    {
        foreach (Monster prefab in monsterPrefabs)
        {
            if (prefab.name == prefabName)
            {
                PoolManager.Instance.CreatePool(key, prefab.gameObject, poolInitialSize);
                return;
            }
        }

        Debug.LogError($"몬스터 프리팹이 없습니다. prefab={prefabName}");
    }
}
