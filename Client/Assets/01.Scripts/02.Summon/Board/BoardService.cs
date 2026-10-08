using System.Collections.Generic;
using UnityEngine;

// 유닛을 배치하는 보드 서비스
// 슬롯 하나에 유닛 하나가 들어가며, 유닛 프리팹은 UnitData.Prefab과 이름이 같아야 한다.
// 슬롯 위치는 BoardView에서 받는다.
public class BoardService : IBoardService
{
    private readonly IDataTableService dataTable;
    private readonly IPoolService pool;
    private readonly BoardView view;
    private readonly Unit[] unitPrefabs;
    private readonly int poolInitialSize;

    private readonly Unit[] units;

    public int SlotCount => units.Length;

    public BoardService(
        IDataTableService dataTable,
        IPoolService pool,
        BoardView view,
        Unit[] unitPrefabs,
        int poolInitialSize)
    {
        this.dataTable = dataTable;
        this.pool = pool;
        this.view = view;
        this.unitPrefabs = unitPrefabs;
        this.poolInitialSize = poolInitialSize;

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

        string key = Unit.GetPoolKey(unitId);
        if (!pool.HasPool(key))
            CreatePool(key, data.Prefab);

        GameObject go = pool.GetObject(key);
        if (go == null)
            return null;

        go.transform.position = view.GetSlotPosition(slotIndex);

        Unit unit = go.GetComponent<Unit>();
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

        pool.ReleaseObject(unit.PoolKey, unit.gameObject);
        units[slotIndex] = null;
    }

    private void CreatePool(string key, string prefabName)
    {
        foreach (Unit prefab in unitPrefabs)
        {
            if (prefab.name == prefabName)
            {
                pool.CreatePool(key, prefab.gameObject, poolInitialSize);
                return;
            }
        }

        Debug.LogError($"유닛 프리팹이 없습니다. prefab={prefabName}");
    }
}
