using UnityEngine;
using CasualGame.Enum;

// 몬스터 이동/공격 클래스
// 스폰 위치에서 본진을 향해 직진하고, 본진 반지름 + 공격 사거리에 닿으면 멈춰서 공격 주기마다 본진을 공격한다.
public class MonsterController : BaseController
{
    protected Monster monster;
    protected ICastleService castle;
    protected float attackTimer;
    protected bool isInitialized;

    protected override void Awake()
    {
        base.Awake();

        monster = GetComponent<Monster>();
        castle = ServiceLocator.Resolve<ICastleService>();
    }

    /// <summary>
    /// 스폰 위치로 이동 후 진격 시작
    /// </summary>
    /// <param name="spawnPosition">스폰 위치</param>
    public void Init(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        attackTimer = 0f;
        isInitialized = true;

        Refresh();
    }

    /// <summary>
    /// 스탯 변경 시 속도 다시 계산
    /// </summary>
    public void Refresh()
    {
        baseSpeed = monster.Stat.MoveSpeed.Value;
        slowSources.Clear();
        CalculateSpeed();
    }

    protected override void Update()
    {
        if (!isInitialized || monster.Condition.IsDead)
            return;

        if (gameState.State != GameState.RUNNING)
            return;

        attackTimer -= Time.deltaTime;

        if (IsInAttackRange())
        {
            moveDirection = Vector2.zero;
            TryAttack();
            return;
        }

        base.Update();
    }

    protected override void Move()
    {
        moveDirection = ((Vector2)(castle.Position - transform.position)).normalized;

        base.Move();
    }

    private bool IsInAttackRange()
    {
        float stopDistance = castle.Radius + monster.Stat.Range.Value;
        return (castle.Position - transform.position).sqrMagnitude <= stopDistance * stopDistance;
    }

    /// <summary>
    /// 공격 주기가 됐으면 본진 공격
    /// </summary>
    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        // 본진 체력은 정수이므로 반올림해서 준다
        castle.TakeDamage(Mathf.RoundToInt(monster.Stat.Attack.Value));
        attackTimer = monster.Stat.AttackInterval;
    }
}
