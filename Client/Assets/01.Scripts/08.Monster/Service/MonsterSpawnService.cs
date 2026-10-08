using System.Collections.Generic;
using UnityEngine;

// 몬스터 생성/반납과 필드 위 몬스터 목록을 관리하는 서비스
// 몬스터 프리팹은 MonsterData.Prefab 값을 키로 풀에서 꺼낸다. (등록은 GameBootstrapper)
// 스폰 위치는 본진 중심 반원 둘레에서 무작위 각도로 정한다.
public class MonsterSpawnService : IMonsterSpawnService
{
    private readonly IDataTableService dataTable;
    private readonly IPrefabPool<Monster> pool;
    private readonly MonsterSpawnAreaView spawnArea;

    private readonly List<Monster> activeMonsters = new List<Monster>();

    public IReadOnlyList<Monster> ActiveMonsters => activeMonsters;
    public int MonsterCount => activeMonsters.Count;

    public MonsterSpawnService(IDataTableService dataTable, IPrefabPool<Monster> pool, MonsterSpawnAreaView spawnArea)
    {
        this.dataTable = dataTable;
        this.pool = pool;
        this.spawnArea = spawnArea;
    }

    /// <summary>
    /// 몬스터 생성 후 스폰 영역의 무작위 지점에 배치
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

        Monster monster = pool.Spawn(data.Prefab);
        if (monster == null)
            return null;

        float angle = Random.Range(spawnArea.MinAngle, spawnArea.MaxAngle);
        monster.Init(data, spawnArea.GetPoint(angle));

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

        pool.Despawn(monster);
        EventBus.OnMonsterCountChanged?.Invoke(activeMonsters.Count);
    }

    /// <summary>
    /// 필드 위 모든 몬스터 반납
    /// </summary>
    public void DespawnAll()
    {
        for (int i = activeMonsters.Count - 1; i >= 0; i--)
            pool.Despawn(activeMonsters[i]);

        activeMonsters.Clear();
        EventBus.OnMonsterCountChanged?.Invoke(0);
    }
}
