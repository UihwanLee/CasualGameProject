// 게임 프로젝트에서 사용할 Enum 타입에 접근할 수 있는 Enum Namespace
// Enum Type은 summary 주석으로 용도를 적고 정의한다.
namespace CasualGame.Enum
{
    /// <summary>
    /// GameState
    /// </summary>
    public enum GameState
    {
        READY,
        RUNNING,
        PAUSE,
        GAMEOVER,
        UNDEF,
    }

    /// <summary>
    /// 유닛 등급
    /// </summary>
    public enum Tier
    {
        COMMON,
        RARE,
        EPIC,
        LEGEND,
        MYTHIC,
    }

    /// <summary>
    /// Attribute 종류
    /// </summary>
    public enum AttributeType
    {
        // Condition
        MaxHp,
        Hp,

        // 공격 (유닛, 몬스터 공용)
        Attack,
        AttackSpeed,
        Range,

        // 이동 (몬스터)
        MoveSpeed,
    }

    /// <summary>
    /// 요청 결과 코드
    /// </summary>
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
}
