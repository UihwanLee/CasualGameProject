using System.Collections.Generic;
using UnityEngine;

// 유닛을 배치하는 보드 서비스
// 슬롯 하나에 유닛 하나가 들어가며, 유닛 프리팹은 UnitData.Prefab 값을 키로 풀에서 꺼낸다. (등록은 GameBootstrapper)
// 슬롯 위치는 BoardView에서 받는다.
public class BoardService : IBoardService
{
    private readonly IDataTableService dataTable;
    private readonly IPrefabPool<Unit> pool;
    private readonly BoardView view;

    private readonly Unit[] units;

    public int SlotCount => units.Length;

    public BoardService(IDataTableService dataTable, IPrefabPool<Unit> pool, BoardView view)
    {
        this.dataTable = dataTable;
        this.pool = pool;
        this.view = view;

        units = new Unit[view.SlotCount];
    }

    public Unit GetUnit(int slotIndex)
    {
        return units[slotIndex];
    }

    /// <summary>
    /// 비어 있는 첫 번째 슬롯 번호 (없으면 -1)
    /// </summary>
    public int FindEmptySlot()
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == null)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// 해당 유닛이 배치된 슬롯 번호 목록
    /// </summary>
    public List<int> FindSlotsWithUnit(int unitId)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] != null && units[i].Data.Id == unitId)
                result.Add(i);
        }

        return result;
    }

    /// <summary>
    /// 슬롯에 유닛 배치
    /// </summary>
    public Unit Place(int slotIndex, int unitId)
    {
        UnitData data = dataTable.GetUnit(unitId);
        if (data == null)
        {
            Debug.LogError($"UnitData가 없습니다. id={unitId}");
            return null;
        }

        Remove(slotIndex);

        Unit unit = pool.Spawn(data.Prefab);
        if (unit == null)
            return null;

        unit.transform.position = view.GetSlotPosition(slotIndex);
        unit.Init(data, slotIndex);
        units[slotIndex] = unit;

        return unit;
    }

    /// <summary>
    /// 슬롯의 유닛을 풀에 반납
    /// </summary>
    public void Remove(int slotIndex)
    {
        Unit unit = units[slotIndex];
        if (unit == null)
            return;

        pool.Despawn(unit);
        units[slotIndex] = null;
    }
}
