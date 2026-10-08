using System;
using UnityEngine;
using CasualGame.Enum;

// 게임 전체 상태를 관리하는 서비스
public class GameStateService : IGameStateService, IDisposable
{
    private readonly IGoldService gold;
    private readonly IWaveService wave;

    public GameState State { get; private set; } = GameState.UNDEF;

    public GameStateService(IGoldService gold, IWaveService wave)
    {
        this.gold = gold;
        this.wave = wave;

        // 타임 스케일 걸려있을 경우에 풀기
        Time.timeScale = 1f;

        State = GameState.READY;

        EventBus.OnMonsterCountChanged += CheckGameOver;
    }

    public void Dispose()
    {
        EventBus.OnMonsterCountChanged -= CheckGameOver;
    }

    /// <summary>
    /// 게임 시작 (골드 초기화 후 첫 웨이브 진행)
    /// </summary>
    public void StartGame()
    {
        gold.Set(Define.START_GOLD);
        EventBus.OnSummonCostChanged?.Invoke(Define.SUMMON_BASE_COST);

        ChangeState(GameState.RUNNING);
        wave.StartWave();
    }

    /// <summary>
    /// 게임 종료 (웨이브 중단)
    /// </summary>
    public void GameOver()
    {
        if (State == GameState.GAMEOVER) return;

        wave.StopWave();
        ChangeState(GameState.GAMEOVER);
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

    /// <summary>
    /// 필드 몬스터 수가 한도를 넘으면 패배
    /// </summary>
    private void CheckGameOver(int monsterCount)
    {
        if (State == GameState.RUNNING && monsterCount > Define.MAX_MONSTER_COUNT)
            GameOver();
    }
}
