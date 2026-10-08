// 게임 프로젝트에서 사용할 상수값에 대해 접근할 수 있는 전역 클래스
// 상수는 용도별로 빈 줄과 주석으로 묶어서 정의한다.
// const -> 상수이므로 그냥 사용해도됨
// static readonly -> Inspector 창에서 바꿀 수 없는 상수값 / 직렬화 불가
// LayerMask도 여기서 정의
using UnityEngine;

public static class Define
{
    // 파일 경로
    public const string FILE_PATH_DATA_TABLE_JSON = "Data/Table/";

    // 게임 규칙
    public const int START_GOLD = 100;             // 시작 골드
    public const int SUMMON_BASE_COST = 20;        // 첫 소환 비용
    public const int SUMMON_COST_INCREASE = 2;     // 소환할 때마다 늘어나는 비용
    public const int MAX_MONSTER_COUNT = 100;      // 필드 몬스터 수가 이 값을 넘으면 패배
    public const int MERGE_REQUIRE_COUNT = 3;      // 합성에 필요한 같은 유닛 수

    // 풀 키
    public const string POOL_KEY_MONSTER = "Monster";      // 실제 키는 Monster_{Id}
    public const string POOL_KEY_UNIT = "Unit";            // 실제 키는 Unit_{Id}
    public const string POOL_KEY_PROJECTILE = "Projectile";
    public const string POOL_KEY_FLOATING_TEXT = "FloatingText";
}
