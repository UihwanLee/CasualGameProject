using System.Collections.Generic;
using UnityEngine;
using CasualGame.Enum;

// 유닛 공격 클래스
// 사거리 안에서 가장 가까운 몬스터를 찾아 공격 주기마다 데미지를 준다.
public class UnitController : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Unit unit;
    private IGameStateService gameState;
    private IMonsterSpawnService monsterSpawn;
    private Monster target;
    private float attackTimer;

    public Monster Target => target;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        gameState = ServiceLocator.Resolve<IGameStateService>();
        monsterSpawn = ServiceLocator.Resolve<IMonsterSpawnService>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init()
    {
        target = null;
        attackTimer = 0f;
    }

    private void Update()
    {
        if (gameState.State != GameState.RUNNING)
            return;

        attackTimer -= Time.deltaTime;

        if (!IsValidTarget(target))
            target = FindNearestTarget();

        if (target == null || attackTimer > 0f)
            return;

        Attack();
        attackTimer = unit.Stat.AttackInterval;
    }

    /// <summary>
    /// 타겟 공격 (투사체 없이 바로 데미지)
    /// </summary>
    private void Attack()
    {
        spriteRenderer.flipX = target.transform.position.x < transform.position.x;
        target.Condition.TakeDamage(unit.Stat.Attack.Value);
    }

    private bool IsValidTarget(Monster monster)
    {
        if (monster == null || !monster.gameObject.activeInHierarchy || monster.Condition.IsDead)
            return false;

        return IsInRange(monster);
    }

    private bool IsInRange(Monster monster)
    {
        float range = unit.Stat.Range.Value;
        return (monster.transform.position - transform.position).sqrMagnitude <= range * range;
    }

    /// <summary>
    /// 사거리 안에서 가장 가까운 몬스터 탐색
    /// </summary>
    private Monster FindNearestTarget()
    {
        Monster nearest = null;
        float nearestSqr = float.MaxValue;
        float range = unit.Stat.Range.Value;
        float rangeSqr = range * range;

        // 인터페이스 리스트를 foreach로 돌면 열거자 박싱이 생기므로 인덱스로 순회한다
        IReadOnlyList<Monster> monsters = monsterSpawn.ActiveMonsters;
        for (int i = 0; i < monsters.Count; i++)
        {
            Monster monster = monsters[i];
            if (monster.Condition.IsDead)
                continue;

            float sqr = (monster.transform.position - transform.position).sqrMagnitude;
            if (sqr <= rangeSqr && sqr < nearestSqr)
            {
                nearest = monster;
                nearestSqr = sqr;
            }
        }

        return nearest;
    }
}
