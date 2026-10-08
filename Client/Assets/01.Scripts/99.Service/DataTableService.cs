using System.Collections.Generic;
using System.Linq;
using CasualGame.Enum;

// 게임 내 오브젝트들의 Data에 접근할 수 있는 서비스
// Dictionary 자료구조를 통해 Data를 가지고 있으며 고유 ID로 Key를 분리한다.
// 시트 연동 전까지는 GameBootstrapper의 Inspector에 입력한 리스트를 데이터로 사용한다.
public class DataTableService : IDataTableService
{
    private readonly Dictionary<int, UnitData> unitDict;
    private readonly Dictionary<int, MonsterData> monsterDict;
    private readonly Dictionary<Tier, List<UnitData>> unitTierDict;
    private readonly List<WaveData> waveList;
    private readonly List<SummonRateData> summonRateList;

    public IReadOnlyDictionary<Tier, List<UnitData>> UnitTierDict => unitTierDict;
    public IReadOnlyList<WaveData> WaveList => waveList;
    public IReadOnlyList<SummonRateData> SummonRateList => summonRateList;

    public DataTableService(
        List<UnitData> unitDataList,
        List<MonsterData> monsterDataList,
        List<WaveData> waveDataList,
        List<SummonRateData> summonRateDataList)
    {
        unitDict = unitDataList.ToDictionary(data => data.Id);
        monsterDict = monsterDataList.ToDictionary(data => data.Id);
        waveList = waveDataList.OrderBy(data => data.Id).ToList();
        summonRateList = summonRateDataList;

        // 소환/합성에서 등급별로 뽑기 위해 나눠둔다
        unitTierDict = unitDataList
            .GroupBy(data => data.Tier)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public UnitData GetUnit(int id)
    {
        return unitDict.TryGetValue(id, out UnitData data) ? data : null;
    }

    public MonsterData GetMonster(int id)
    {
        return monsterDict.TryGetValue(id, out MonsterData data) ? data : null;
    }
}
