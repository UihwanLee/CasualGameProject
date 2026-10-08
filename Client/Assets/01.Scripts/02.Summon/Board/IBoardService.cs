using System.Collections.Generic;

// 보드 슬롯 상태 조회와 유닛 배치/제거
public interface IBoardService
{
    int SlotCount { get; }

    Unit GetUnit(int slotIndex);
    int FindEmptySlot();
    List<int> FindSlotsWithUnit(int unitId);
    Unit Place(int slotIndex, int unitId);
    void Remove(int slotIndex);
}
