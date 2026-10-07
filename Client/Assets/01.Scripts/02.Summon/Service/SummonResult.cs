using CasualGame.Enum;

// 소환 요청 결과
public struct SummonResult
{
    public ErrorCode Code;          // 결과 코드
    public int UnitId;              // 소환된 유닛 ID
    public int SlotIndex;           // 배치할 슬롯
    public int Gold;                // 소환 후 골드
    public int NextCost;            // 다음 소환 비용
}

// 합성 요청 결과
public struct MergeResult
{
    public ErrorCode Code;          // 결과 코드
    public int[] ConsumedSlots;     // 재료로 사라질 슬롯
    public int UnitId;              // 합성된 유닛 ID
    public int SlotIndex;           // 합성된 유닛을 배치할 슬롯
}
