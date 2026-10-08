using UnityEngine;
using CasualGame.Enum;

// 개발용 이벤트 로그 (UI 없이 게임 진행을 Console로 확인하기 위한 용도)
// 에디터와 개발 빌드에서만 동작한다.
public class DebugEventLog : MonoBehaviour
{
    private const string LOG_PREFIX = "[Game]";
    private const int MONSTER_COUNT_LOG_STEP = 10;     // 몬스터 수는 이 단위로만 출력 (스폰/처치마다 찍히면 로그가 너무 많음)

    private void Awake()
    {
        if (!Debug.isDebugBuild)
            enabled = false;
    }

    private void OnEnable()
    {
        EventBus.OnGameStateChanged += LogGameState;
        EventBus.OnGoldChanged += LogGold;
        EventBus.OnSummonCostChanged += LogSummonCost;
        EventBus.OnWaveStart += LogWaveStart;
        EventBus.OnMonsterCountChanged += LogMonsterCount;
        EventBus.OnRequestFailed += LogRequestFailed;
    }

    private void OnDisable()
    {
        EventBus.OnGameStateChanged -= LogGameState;
        EventBus.OnGoldChanged -= LogGold;
        EventBus.OnSummonCostChanged -= LogSummonCost;
        EventBus.OnWaveStart -= LogWaveStart;
        EventBus.OnMonsterCountChanged -= LogMonsterCount;
        EventBus.OnRequestFailed -= LogRequestFailed;
    }

    private void LogGameState(GameState state)
    {
        Debug.Log($"{LOG_PREFIX} 상태: {state}");
    }

    private void LogGold(int gold)
    {
        Debug.Log($"{LOG_PREFIX} 골드: {gold}");
    }

    private void LogSummonCost(int cost)
    {
        Debug.Log($"{LOG_PREFIX} 소환 비용: {cost}");
    }

    private void LogWaveStart(int wave)
    {
        Debug.Log($"{LOG_PREFIX} 웨이브 {wave} 시작");
    }

    private void LogMonsterCount(int count)
    {
        if (count % MONSTER_COUNT_LOG_STEP == 0)
            Debug.Log($"{LOG_PREFIX} 몬스터 수: {count}");
    }

    private void LogRequestFailed(ErrorCode code)
    {
        Debug.LogWarning($"{LOG_PREFIX} 요청 실패: {code}");
    }
}
