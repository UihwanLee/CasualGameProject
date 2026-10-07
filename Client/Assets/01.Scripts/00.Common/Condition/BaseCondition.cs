using System.Collections.Generic;
using UnityEngine;
using CasualGame.Enum;

// 체력을 가진 오브젝트에서 사용할 BaseCondition 클래스
// Condition에서 관리할 Attribute는 다음과 같다.
//   maxHp
//   hp
public class BaseCondition : MonoBehaviour, IDamageable
{
    [SerializeField] protected StatAttribute maxHp;
    [SerializeField] protected StatAttribute hp;

    protected Dictionary<AttributeType, StatAttribute> conditionDict = new Dictionary<AttributeType, StatAttribute>();

    public bool IsDead { get; protected set; }

    public virtual void InitCondition(float _maxHp)
    {
        maxHp = new StatAttribute(0, _maxHp, 0f);
        hp = new StatAttribute(1, _maxHp, 0f);

        conditionDict.Clear();
        conditionDict.Add(AttributeType.MaxHp, maxHp);
        conditionDict.Add(AttributeType.Hp, hp);

        IsDead = false;
    }

    public virtual void Add(AttributeType type, float amount)
    {
        if (conditionDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Add(amount);
        }
    }

    public virtual void Sub(AttributeType type, float amount)
    {
        if (conditionDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Sub(amount);
        }
    }

    public virtual void Set(AttributeType type, float value)
    {
        if (conditionDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Set(value);
        }
    }

    public void ResetValue()
    {
        foreach (var attribute in conditionDict.Values)
        {
            attribute.ResetValue();
        }
    }

    /// <summary>
    /// 일반적인 데미지 호출
    /// </summary>
    /// <param name="damage">받을 데미지</param>
    public virtual void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        // hp는 StatAttribute라 Sub가 additive를 깎으므로 Set으로 직접 반영
        hp.Set(Mathf.Max(hp.Value - damage, 0f));

        if (hp.Value <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 사망 처리 (자식 클래스에서 구현)
    /// </summary>
    protected virtual void Die()
    {
        IsDead = true;
    }

    #region 프로퍼티
    public Attribute MaxHp => maxHp;
    public Attribute Hp => hp;
    #endregion
}
