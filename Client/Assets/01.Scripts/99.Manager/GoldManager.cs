using UnityEngine;

// 골드를 관리하는 매니저 (씬마다 하나)
// 소환 결과로 받은 골드를 반영하고, 몬스터 처치 골드를 적립한다.
public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.OnMonsterKilled += OnMonsterKilled;
    }

    private void OnDisable()
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
