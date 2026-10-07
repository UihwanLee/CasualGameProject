using UnityEngine;

// 몬스터가 순환할 경로 (씬에 배치한 웨이포인트 목록)
// 0번이 스폰 지점이고, 마지막 지점 다음은 다시 0번으로 이어진다.
public class MonsterPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public int Count => waypoints.Length;

    public Vector3 GetPoint(int index)
    {
        return waypoints[index].position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform from = waypoints[i];
            Transform to = waypoints[(i + 1) % waypoints.Length];

            if (from != null && to != null)
                Gizmos.DrawLine(from.position, to.position);
        }
    }
#endif
}
