using UnityEngine;

// 몬스터 스폰 영역을 씬에서 지정하는 컴포넌트 (씬마다 하나)
// 본진을 중심으로 한 반원 둘레에서 스폰한다. 본진까지 거리가 모두 같아 방향과 상관없이 도착 시간이 같다.
// 각도는 오른쪽(3시)이 0도, 위쪽(12시)이 90도, 왼쪽(9시)이 180도다.
public class MonsterSpawnAreaView : MonoBehaviour
{
    private const int GIZMO_SEGMENT_COUNT = 32;

    [Header("중심 (본진)")]
    [SerializeField] private CastleView castle;

    [Header("스폰 반원")]
    [SerializeField] private float radius = 10f;
    [SerializeField] [Range(0f, 180f)] private float minAngle = 20f;
    [SerializeField] [Range(0f, 180f)] private float maxAngle = 160f;

    public float MinAngle => minAngle;
    public float MaxAngle => maxAngle;

    /// <summary>
    /// 반원 둘레 위의 지점
    /// </summary>
    /// <param name="angle">각도(도)</param>
    public Vector3 GetPoint(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return castle.Position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (castle == null)
            return;

        Gizmos.color = Color.red;
        Vector3 prev = GetPoint(minAngle);
        for (int i = 1; i <= GIZMO_SEGMENT_COUNT; i++)
        {
            Vector3 next = GetPoint(Mathf.Lerp(minAngle, maxAngle, (float)i / GIZMO_SEGMENT_COUNT));
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
#endif
}
