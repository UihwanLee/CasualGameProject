using UnityEngine;

// 유닛 컴포넌트들을 묶어서 접근할 수 있는 허브 클래스
// Stat(공격력, 공격 속도, 사거리), Controller(타겟 탐색, 공격)로 역할을 나눈다.
public class Unit : MonoBehaviour
{
    [SerializeField] private UnitController controller;
    [SerializeField] private UnitStat stat;

    private UnitData data;

    public UnitController Controller => controller;
    public UnitStat Stat => stat;
    public UnitData Data => data;
    public int SlotIndex { get; private set; }
    public string PoolKey => GetPoolKey(data.Id);

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<UnitController>();

        if (stat == null)
            stat = GetComponent<UnitStat>();
    }

    /// <summary>
    /// 풀에서 꺼낼 때마다 데이터로 초기화
    /// </summary>
    /// <param name="_data">유닛 데이터</param>
    /// <param name="slotIndex">배치된 보드 슬롯 번호</param>
    public void Init(UnitData _data, int slotIndex)
    {
        data = _data;
        SlotIndex = slotIndex;

        stat.InitStat(data);
        controller.Init();
    }

    public static string GetPoolKey(int unitId)
    {
        return $"{Define.POOL_KEY_UNIT}_{unitId}";
    }
}
