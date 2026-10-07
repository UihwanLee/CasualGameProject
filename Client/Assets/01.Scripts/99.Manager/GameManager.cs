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

    private void OnEnable()
    {
        EventBus.OnMonsterCountChanged += CheckGameOver;
    }

    private void OnDisable()
    {
        EventBus.OnMonsterCountChanged -= CheckGameOver;
    }

    /// <summary>
    /// 게임 시작 (골드 초기화 후 첫 웨이브 진행)
    /// </summary>
    public void StartGame()
    {
        GoldManager.Instance.Set(Define.START_GOLD);
        EventBus.OnSummonCostChanged?.Invoke(Define.SUMMON_BASE_COST);

        ChangeState(GameState.RUNNING);
        WaveManager.Instance.StartWave();
    }

    /// <summary>
    /// 게임 종료 (웨이브 중단)
    /// </summary>
    public void GameOver()
    {
        if (State == GameState.GAMEOVER) return;

        WaveManager.Instance.StopWave();
        ChangeState(GameState.GAMEOVER);
    }

    /// <summary>
    /// 필드 몬스터 수가 한도를 넘으면 패배
    /// </summary>
    private void CheckGameOver(int monsterCount)
    {
        if (State == GameState.RUNNING && monsterCount > Define.MAX_MONSTER_COUNT)
            GameOver();
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
