using UnityEngine;

// 몬스터 컴포넌트들을 묶어서 접근할 수 있는 허브 클래스
// Stat(이동속도), Condition(체력, 사망), Controller(경로 이동)로 역할을 나눈다.
public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterController controller;
    [SerializeField] private MonsterStat stat;
    [SerializeField] private MonsterCondition condition;

    private MonsterData data;

    public MonsterController Controller => controller;
    public MonsterStat Stat => stat;
    public MonsterCondition Condition => condition;
    public MonsterData Data => data;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<MonsterController>();

        if (stat == null)
            stat = GetComponent<MonsterStat>();

        if (condition == null)
            condition = GetComponent<MonsterCondition>();
    }

    /// <summary>
    /// 풀에서 꺼낼 때마다 데이터로 초기화
    /// </summary>
    /// <param name="_data">몬스터 데이터</param>
    /// <param name="path">따라갈 경로</param>
    public void Init(MonsterData _data, MonsterPath path)
    {
        data = _data;

        stat.InitStat(data);
        condition.InitMonsterCondition(data);
        controller.Init(path);
    }
}
