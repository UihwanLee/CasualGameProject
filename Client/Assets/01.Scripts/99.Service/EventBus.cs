using System;
using CasualGame.Enum;

// 시스템 간 결합도를 낮추기 위한 전역 이벤트 모음
// 구독한 쪽은 OnDisable/OnDestroy에서 반드시 구독 해제한다.
public static class EventBus
{
    public static Action<GameState> OnGameStateChanged;     // 게임 상태 변경 시 발생
    public static Action<int> OnGoldChanged;                // 골드 변경 시 발생 (현재 골드)
    public static Action<int> OnSummonCostChanged;          // 소환 비용 변경 시 발생 (현재 비용)
    public static Action<int> OnWaveStart;                  // 웨이브 시작 시 발생 (웨이브 번호)
    public static Action<int> OnMonsterCountChanged;        // 필드 몬스터 수 변경 시 발생
    public static Action<int> OnCastleHpChanged;            // 본진 체력 변경 시 발생 (현재 체력)
    public static Action<Monster> OnMonsterKilled;          // 몬스터 처치 시 발생
    public static Action<ErrorCode> OnRequestFailed;        // 소환/합성 요청 실패 시 발생
}
