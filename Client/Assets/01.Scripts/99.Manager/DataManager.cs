using System.Collections.Generic;
using System.Linq;
using CasualGame.Enum;
using UnityEngine;

// 게임 내 오브젝트들의 Data에 접근할 수 있는 전역 클래스
// Dictionary 자료구조를 통해 Data를 가지고 있으며 고유 ID로 Key를 분리한다.
// 시트 연동 전까지는 Inspector에 입력한 리스트를 데이터로 사용한다.
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("임시 데이터 (시트 연동 전)")]
    [SerializeField] private List<UnitData> unitDataList = new List<UnitData>();
    [SerializeField] private List<MonsterData> monsterDataList = new List<MonsterData>();
    [SerializeField] private List<WaveData> waveDataList = new List<WaveData>();
    [SerializeField] private List<SummonRateData> summonRateDataList = new List<SummonRateData>();

    public static Dictionary<int, UnitData> UnitDict { get; private set; }
    public static Dictionary<int, MonsterData> MonsterDict { get; private set; }
    public static Dictionary<Tier, List<UnitData>> UnitTierDict { get; private set; }
    public static List<WaveData> WaveList { get; private set; }
    public static List<SummonRateData> SummonRateList { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAllData();
    }

    /// <summary>
    /// 모든 데이터 테이블 로드
    /// </summary>
    private void LoadAllData()
    {
        UnitDict = unitDataList.ToDictionary(data => data.Id);
        MonsterDict = monsterDataList.ToDictionary(data => data.Id);
        WaveList = waveDataList.OrderBy(data => data.Id).ToList();
        SummonRateList = summonRateDataList;

        // 소환/합성에서 등급별로 뽑기 위해 나눠둔다
        UnitTierDict = unitDataList
            .GroupBy(data => data.Tier)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public static UnitData GetUnit(int id)
    {
        return UnitDict.TryGetValue(id, out UnitData data) ? data : null;
    }

    public static MonsterData GetMonster(int id)
    {
        return MonsterDict.TryGetValue(id, out MonsterData data) ? data : null;
    }
}
