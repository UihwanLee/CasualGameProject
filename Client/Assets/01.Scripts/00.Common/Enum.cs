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

    /// <summary>
    /// Attribute 종류
    /// </summary>
    #region AttributeType

    public enum AttributeType
    {
        // Condition
        MaxHp,
        Hp,

        // Unit Stat
        Attack,
        AttackSpeed,
        Range,

        // Monster Stat
        MoveSpeed,
    }

    #endregion

    /// <summary>
    /// 요청 결과 코드
    /// </summary>
    #region ErrorCode

    public enum ErrorCode
    {
        Ok = 0,
        NotEnoughGold = 1001,
        BoardFull = 1002,
        InvalidMerge = 1003,
        DuplicateSeq = 2001,
        SessionExpired = 3001,
        ServerError = 9000,
    }

    #endregion
}
