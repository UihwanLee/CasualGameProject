using UnityEngine;
using CasualGame.Enum;

// 게임 전체 상태를 관리하는 전역 매니저
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public GameState State { get; private set; } = GameState.UNDEF;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 타임 스케일 걸려있을 경우에 풀기
        Time.timeScale = 1f;

        State = GameState.READY;
    }

    /// <summary>
    /// 게임 상태 변경
    /// </summary>
    /// <param name="state">변경할 상태</param>
    public void ChangeState(GameState state)
    {
        if (State == state) return;

        State = state;
        EventBus.OnGameStateChanged?.Invoke(state);
    }
}
