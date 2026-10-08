using UnityEngine;

// 몬스터 체력, 사망 처리 클래스
public class MonsterCondition : BaseCondition
{
    protected Monster monster;
    protected IMonsterSpawnService monsterSpawn;

    protected virtual void Awake()
    {
        monster = GetComponent<Monster>();
        monsterSpawn = ServiceLocator.Resolve<IMonsterSpawnService>();
    }

    public void InitMonsterCondition(MonsterData data)
    {
        InitCondition(data.Hp);
    }

    protected override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        EventBus.OnMonsterKilled?.Invoke(monster);

        monsterSpawn.Despawn(monster);
    }
}
