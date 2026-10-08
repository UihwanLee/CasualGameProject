using UnityEngine;

// 본진 체력을 관리하는 서비스
// 체력이 바뀌면 OnCastleHpChanged를 발생시키고, 0이 되면 GameStateService가 패배 처리한다.
public class CastleService : ICastleService
{
    private readonly CastleView view;

    public int Hp { get; private set; }
    public int MaxHp { get; }
    public bool IsDestroyed => Hp <= 0;
    public Vector3 Position => view.Position;
    public float Radius => view.Radius;

    public CastleService(CastleView view, int maxHp)
    {
        this.view = view;
        MaxHp = maxHp;
        Hp = maxHp;
    }

    /// <summary>
    /// 체력을 최대치로 회복 (게임 시작 시)
    /// </summary>
    public void ResetHp()
    {
        SetHp(MaxHp);
    }

    /// <summary>
    /// 본진 피격 (이미 파괴됐으면 무시)
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public void TakeDamage(int damage)
    {
        if (IsDestroyed || damage <= 0)
            return;

        SetHp(Hp - damage);
    }

    private void SetHp(int hp)
    {
        Hp = Mathf.Clamp(hp, 0, MaxHp);
        EventBus.OnCastleHpChanged?.Invoke(Hp);
    }
}
