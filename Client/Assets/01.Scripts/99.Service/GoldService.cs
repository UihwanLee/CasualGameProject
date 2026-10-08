using System;
using UnityEngine;

// 골드를 관리하는 서비스
// 소환 결과로 받은 골드를 반영하고, 몬스터 처치 골드를 적립한다.
public class GoldService : IGoldService, IDisposable
{
    public int Gold { get; private set; }

    public GoldService()
    {
        EventBus.OnMonsterKilled += OnMonsterKilled;
    }

    public void Dispose()
    {
        EventBus.OnMonsterKilled -= OnMonsterKilled;
    }

    public void Set(int gold)
    {
        Gold = Mathf.Max(gold, 0);
        EventBus.OnGoldChanged?.Invoke(Gold);
    }

    public void Add(int amount)
    {
        Set(Gold + amount);
    }

    private void OnMonsterKilled(Monster monster)
    {
        Add(monster.Data.Gold);
    }
}
