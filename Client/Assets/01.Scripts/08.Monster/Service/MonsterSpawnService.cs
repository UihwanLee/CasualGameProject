using System.Collections.Generic;
using UnityEngine;

// 몬스터 생성/반납과 필드 위 몬스터 목록을 관리하는 서비스
// 몬스터 프리팹은 이름으로 찾으며, MonsterData.Prefab과 프리팹 이름이 같아야 한다.
public class MonsterSpawnService : IMonsterSpawnService
{
    private readonly IDataTableService dataTable;
    private readonly IPoolService pool;
    private readonly MonsterPath path;
    private readonly Monster[] monsterPrefabs;
    private readonly int poolInitialSize;

    private readonly List<Monster> activeMonsters = new List<Monster>();

    public IReadOnlyList<Monster> ActiveMonsters => activeMonsters;
    public int MonsterCount => activeMonsters.Count;

    public MonsterSpawnService(
        IDataTableService dataTable,
        IPoolService pool,
        MonsterPath path,
        Monster[] monsterPrefabs,
        int poolInitialSize)
    {
        this.dataTable = dataTable;
        this.pool = pool;
        this.path = path;
        this.monsterPrefabs = monsterPrefabs;
        this.poolInitialSize = poolInitialSize;
    }

    /// <summary>
    /// 몬스터 생성 후 경로 시작 지점에 배치
    /// </summary>
    /// <param name="monsterId">몬스터 ID</param>
    public Monster Spawn(int monsterId)
    {
        MonsterData data = dataTable.GetMonster(monsterId);
        if (data == null)
        {
            Debug.LogError($"MonsterData가 없습니다. id={monsterId}");
            return null;
        }

        string key = Monster.GetPoolKey(monsterId);
        if (!pool.HasPool(key))
            CreatePool(key, data.Prefab);

        GameObject go = pool.GetObject(key);
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

        pool.ReleaseObject(monster.PoolKey, monster.gameObject);
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
            pool.ReleaseObject(monster.PoolKey, monster.gameObject);
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
                pool.CreatePool(key, prefab.gameObject, poolInitialSize);
                return;
            }
        }

        Debug.LogError($"몬스터 프리팹이 없습니다. prefab={prefabName}");
    }
}
