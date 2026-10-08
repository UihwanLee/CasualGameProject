using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CasualGame.Enum;

// 클라이언트 안에서 소환/합성 결과를 결정하는 구현체
// 보드와 골드를 읽어서 결과만 만들고, 실제 반영은 SummonService가 한다.
public class LocalSummonApi : ISummonApi
{
    private readonly IBoardService board;
    private readonly IGoldService gold;
    private readonly IDataTableService dataTable;
    private int summonCount;

    public LocalSummonApi(IBoardService board, IGoldService gold, IDataTableService dataTable)
    {
        this.board = board;
        this.gold = gold;
        this.dataTable = dataTable;
    }

    public int CurrentCost => Define.SUMMON_BASE_COST + Define.SUMMON_COST_INCREASE * summonCount;

    public UniTask<SummonResult> SummonAsync(CancellationToken token)
    {
        int cost = CurrentCost;
        int currentGold = gold.Gold;

        if (currentGold < cost)
            return UniTask.FromResult(new SummonResult { Code = ErrorCode.NotEnoughGold, Gold = currentGold, NextCost = cost });

        int slotIndex = board.FindEmptySlot();
        if (slotIndex < 0)
            return UniTask.FromResult(new SummonResult { Code = ErrorCode.BoardFull, Gold = currentGold, NextCost = cost });

        summonCount++;

        return UniTask.FromResult(new SummonResult
        {
            Code = ErrorCode.Ok,
            UnitId = PickUnit(RollTier()).Id,
            SlotIndex = slotIndex,
            Gold = currentGold - cost,
            NextCost = CurrentCost,
        });
    }

    public UniTask<MergeResult> MergeAsync(int slotIndex, CancellationToken token)
    {
        MergeResult fail = new MergeResult { Code = ErrorCode.InvalidMerge };

        Unit selected = board.GetUnit(slotIndex);
        if (selected == null)
            return UniTask.FromResult(fail);

        // 다음 등급 유닛이 없으면 합성 불가
        Tier nextTier = selected.Data.Tier + 1;
        if (!dataTable.UnitTierDict.ContainsKey(nextTier))
            return UniTask.FromResult(fail);

        List<int> sameSlots = board.FindSlotsWithUnit(selected.Data.Id);
        if (sameSlots.Count < Define.MERGE_REQUIRE_COUNT)
            return UniTask.FromResult(fail);

        // 선택한 슬롯을 포함해서 재료를 고른다
        sameSlots.Remove(slotIndex);
        int[] consumed = new int[Define.MERGE_REQUIRE_COUNT];
        consumed[0] = slotIndex;
        for (int i = 1; i < consumed.Length; i++)
            consumed[i] = sameSlots[i - 1];

        return UniTask.FromResult(new MergeResult
        {
            Code = ErrorCode.Ok,
            ConsumedSlots = consumed,
            UnitId = PickUnit(nextTier).Id,
            SlotIndex = slotIndex,
        });
    }

    /// <summary>
    /// 소환 확률표에 따라 등급 추첨 (유닛이 없는 등급은 제외)
    /// </summary>
    private Tier RollTier()
    {
        // 인터페이스 리스트를 foreach로 돌면 열거자 박싱이 생기므로 인덱스로 순회한다
        IReadOnlyList<SummonRateData> rates = dataTable.SummonRateList;

        float total = 0f;
        for (int i = 0; i < rates.Count; i++)
        {
            SummonRateData rate = rates[i];
            if (dataTable.UnitTierDict.ContainsKey(rate.Tier))
                total += rate.Rate;
        }

        float roll = Random.Range(0f, total);
        for (int i = 0; i < rates.Count; i++)
        {
            SummonRateData rate = rates[i];
            if (!dataTable.UnitTierDict.ContainsKey(rate.Tier))
                continue;

            roll -= rate.Rate;
            if (roll < 0f)
                return rate.Tier;
        }

        return Tier.COMMON;
    }

    /// <summary>
    /// 해당 등급 유닛 중 하나를 무작위로 선택
    /// </summary>
    private UnitData PickUnit(Tier tier)
    {
        List<UnitData> list = dataTable.UnitTierDict[tier];
        return list[Random.Range(0, list.Count)];
    }
}
