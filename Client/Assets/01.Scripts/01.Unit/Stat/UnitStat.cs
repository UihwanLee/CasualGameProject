using System.Collections.Generic;
using UnityEngine;
using CasualGame.Enum;

// 유닛 스탯 관리 클래스
public class UnitStat : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private int id;
    [SerializeField] private StatAttribute attack;
    [SerializeField] private StatAttribute attackSpeed;
    [SerializeField] private StatAttribute range;

    // Stat Attribute Dictionary
    private Dictionary<AttributeType, StatAttribute> attributeDict = new Dictionary<AttributeType, StatAttribute>();

    public void InitStat(UnitData data)
    {
        id = data.Id;
        attack = new StatAttribute(0, data.Attack, 0f);
        attackSpeed = new StatAttribute(1, data.AttackSpeed, 0.01f);
        range = new StatAttribute(2, data.Range, 0f);

        attributeDict.Clear();
        attributeDict.Add(AttributeType.Attack, attack);
        attributeDict.Add(AttributeType.AttackSpeed, attackSpeed);
        attributeDict.Add(AttributeType.Range, range);
    }

    public void Add(AttributeType type, float amount)
    {
        if (attributeDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Add(amount);
        }
    }

    public void Sub(AttributeType type, float amount)
    {
        if (attributeDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Sub(amount);
        }
    }

    public void Set(AttributeType type, float amount)
    {
        if (attributeDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.Set(amount);
        }
    }

    public void AddMultiplier(AttributeType type, float amount)
    {
        if (attributeDict.TryGetValue(type, out StatAttribute attribute))
        {
            attribute.AddMultiplier(amount);
        }
    }

    public void ResetValue()
    {
        foreach (var attribute in attributeDict.Values)
        {
            attribute.ResetValue();
        }
    }

    public int ID { get { return id; } }
    public Attribute Attack { get { return attack; } }
    public Attribute AttackSpeed { get { return attackSpeed; } }
    public Attribute Range { get { return range; } }

    // 공격 한 번에 걸리는 시간(초)
    public float AttackInterval { get { return 1f / attackSpeed.Value; } }
}
