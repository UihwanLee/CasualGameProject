using UnityEngine;

// 본진 체력과 피격
public interface ICastleService
{
    int Hp { get; }
    int MaxHp { get; }
    bool IsDestroyed { get; }
    Vector3 Position { get; }
    float Radius { get; }

    void ResetHp();
    void TakeDamage(int damage);
}
