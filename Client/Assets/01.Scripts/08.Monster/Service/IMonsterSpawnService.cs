using System.Collections.Generic;

// 몬스터 생성/반납과 필드 위 몬스터 목록 관리
public interface IMonsterSpawnService
{
    IReadOnlyList<Monster> ActiveMonsters { get; }
    int MonsterCount { get; }

    Monster Spawn(int monsterId);
    void Despawn(Monster monster);
    void DespawnAll();
}
