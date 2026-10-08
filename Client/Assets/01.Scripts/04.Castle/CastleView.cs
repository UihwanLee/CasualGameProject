using UnityEngine;

// 본진 위치와 크기를 씬에서 지정하는 컴포넌트 (씬마다 하나, 화면 하단 중앙에 배치)
// 체력과 피격 처리는 CastleService가 담당하고, 여기서는 위치와 크기만 제공한다.
public class CastleView : MonoBehaviour
{
    [Header("본진 반지름 (몬스터가 이 거리 + 공격 사거리에서 멈춰 공격)")]
    [SerializeField] private float radius = 1f;

    public Vector3 Position => transform.position;
    public float Radius => radius;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
