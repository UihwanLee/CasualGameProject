using UnityEngine;

// 몬스터 이동 클래스
// MonsterPath의 웨이포인트를 순서대로 따라가며, 마지막 지점에 닿으면 처음으로 돌아가 계속 순환한다.
public class MonsterController : BaseController
{
    [Header("웨이포인트 도착 판정 거리")]
    [SerializeField] private float arriveDistance = 0.05f;

    protected Monster monster;
    protected MonsterPath path;
    protected int targetIndex;

    protected override void Awake()
    {
        base.Awake();

        monster = GetComponent<Monster>();
    }

    /// <summary>
    /// 경로 지정 후 시작 지점으로 이동
    /// </summary>
    /// <param name="_path">따라갈 경로</param>
    public void Init(MonsterPath _path)
    {
        path = _path;
        targetIndex = 1 % path.Count;
        transform.position = path.GetPoint(0);

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
        if (path == null || monster.Condition.IsDead)
            return;

        base.Update();
    }

    protected override void Move()
    {
        Vector2 target = path.GetPoint(targetIndex);
        Vector2 current = transform.position;
        Vector2 toTarget = target - current;

        moveDirection = toTarget.normalized;

        // 이번 프레임에 목표를 지나치면 목표 지점에 맞추고 다음 웨이포인트로
        float step = currentSpeed * Time.deltaTime;
        if (toTarget.magnitude <= Mathf.Max(step, arriveDistance))
        {
            transform.position = target;
            targetIndex = (targetIndex + 1) % path.Count;
            return;
        }

        base.Move();
    }
}
