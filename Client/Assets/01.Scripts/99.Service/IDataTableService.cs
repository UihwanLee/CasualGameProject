using System.Collections.Generic;
using CasualGame.Enum;

// 게임 데이터 테이블 조회
public interface IDataTableService
{
    IReadOnlyDictionary<Tier, List<UnitData>> UnitTierDict { get; }
    IReadOnlyList<WaveData> WaveList { get; }
    IReadOnlyList<SummonRateData> SummonRateList { get; }

    UnitData GetUnit(int id);
    MonsterData GetMonster(int id);
}
