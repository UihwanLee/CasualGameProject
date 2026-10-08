using CasualGame.Enum;

// 게임 전체 상태와 시작/패배 흐름
public interface IGameStateService
{
    GameState State { get; }

    void StartGame();
    void GameOver();
    void ChangeState(GameState state);
}
