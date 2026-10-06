// 게임 프로젝트에서 사용할 Enum 타입에 접근할 수 있는 Enum Namespace
// Enum Type을 정의 할 때 region/endregion을 활용하여 사용한다.
namespace CasualGame.Enum
{
    /// <summary>
    /// GameState
    /// </summary>
    #region GameState

    public enum GameState
    {
        READY,
        RUNNING,
        PAUSE,
        GAMEOVER,
        UNDEF,
    }

    #endregion

    /// <summary>
    /// 유닛 등급
    /// </summary>
    #region Tier

    public enum Tier
    {
        COMMON,
        RARE,
        EPIC,
        LEGEND,
        MYTHIC,
    }

    #endregion
}
