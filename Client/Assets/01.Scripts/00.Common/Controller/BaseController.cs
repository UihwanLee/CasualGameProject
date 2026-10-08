using System.Collections.Generic;
using UnityEngine;
using CasualGame.Enum;

// 이동하는 오브젝트에서 사용할 BaseController 클래스
// 경로를 따라 움직이는 디펜스 게임이라 벽 충돌, 넉백은 제외하고 이동/Flip/슬로우만 다룬다.
public class BaseController : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("Movement")]
    // 이동 방향
    [SerializeField] protected Vector2 moveDirection;
    // 기본 이동 속도
    [SerializeField] protected float baseSpeed;
    [SerializeField] protected float currentSpeed;

    // 슬로우를 건 대상별 감소 비율
    protected Dictionary<object, float> slowSources = new Dictionary<object, float>();

    protected IGameStateService gameState;

    public Vector2 MoveDirection => moveDirection;
    public float BaseSpeed => baseSpeed;
    public float CurrentSpeed => currentSpeed;

    protected virtual void Reset()
    {
        moveDirection = Vector2.zero;
    }

    protected virtual void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        gameState = ServiceLocator.Resolve<IGameStateService>();
    }

    protected virtual void OnEnable()
    {
        moveDirection = Vector2.zero;
    }

    protected virtual void Start()
    {
        CalculateSpeed();
    }

    protected virtual void Update()
    {
        if (gameState.State != GameState.RUNNING)
            return;

        Move();
        Flip();
    }

    /// <summary>
    /// Transform 기반 Move<br/>
    /// moveDirection * currentSpeed 만큼 이동
    /// </summary>
    protected virtual void Move()
    {
        transform.position += (Vector3)(moveDirection * currentSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 이동 방향에 따른 Sprite Flip
    /// </summary>
    protected virtual void Flip()
    {
        if (spriteRenderer == null) return;

        // 이동 중이 아닐 때는 플립 금지
        if (Mathf.Abs(moveDirection.x) < 0.01f)
            return;

        spriteRenderer.flipX = moveDirection.x < 0;
    }

    /// <summary>
    /// 이동속도 감소
    /// </summary>
    /// <param name="source">슬로우를 건 대상</param>
    /// <param name="ratio">감소 비율(0 ~ 1)</param>
    public void ApplySlow(object source, float ratio)
    {
        slowSources[source] = Mathf.Clamp01(ratio);
        CalculateSpeed();
    }

    /// <summary>
    /// 속도 복원
    /// </summary>
    /// <param name="source">슬로우를 건 대상</param>
    public void RemoveSlow(object source)
    {
        if (slowSources.Remove(source))
            CalculateSpeed();
    }

    /// <summary>
    /// 현재의 속도 계산
    /// </summary>
    protected virtual void CalculateSpeed()
    {
        float speed = baseSpeed;

        foreach (float ratio in slowSources.Values)
        {
            speed *= (1f - ratio);
        }

        // 최소 속도 제한
        currentSpeed = Mathf.Max(speed, 0.1f);
    }
}
