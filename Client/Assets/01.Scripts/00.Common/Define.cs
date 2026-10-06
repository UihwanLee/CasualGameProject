// 게임 프로젝트에서 사용할 상수값에 대해 접근할 수 있는 전역 클래스
// 상수를 정의 할 때 region/endregion을 활용하여 사용한다.
// const -> 상수이므로 그냥 사용해도됨
// static readonly -> Inspector 창에서 바꿀 수 없는 상수값 / 직렬화 불가
// LayerMask도 여기서 정의
using UnityEngine;

public static class Define
{
    #region 파일 Define

    public const string FILE_PATH_DATA_TABLE_JSON = "Data/Table/";

    #endregion

    #region GameRule

    public const int MAX_MONSTER_COUNT = 100;      // 필드 몬스터 수가 이 값을 넘으면 패배
    public const int MERGE_REQUIRE_COUNT = 3;      // 합성에 필요한 같은 유닛 수

    #endregion

    #region PoolKey

    public const string POOL_KEY_MONSTER = "Monster";
    public const string POOL_KEY_PROJECTILE = "Projectile";
    public const string POOL_KEY_FLOATING_TEXT = "FloatingText";

    #endregion
}
