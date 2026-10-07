using UnityEngine;

// 몬스터 체력, 사망 처리 클래스
public class MonsterCondition : BaseCondition
{
    protected Monster monster;

    #region LifeCycle

    protected virtual void Awake()
    {
        monster = GetComponent<Monster>();
    }

    #endregion

    #region 초기화

    public void InitMonsterCondition(MonsterData data)
    {
        InitCondition(data.Hp);
    }

    #endregion

    #region 사망 처리

    protected override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        EventBus.OnMonsterKilled?.Invoke(monster);

        SpawnManager.Instance.Despawn(monster);
    }

    #endregion
}
