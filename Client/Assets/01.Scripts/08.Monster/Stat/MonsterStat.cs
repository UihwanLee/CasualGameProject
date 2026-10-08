using System.Collections.Generic;
using UnityEngine;
using CasualGame.Enum;

// 몬스터 스탯 관리 클래스
// 슬로우 등 디버프가 붙을 수 있는 값만 StatAttribute로 관리한다.
public class MonsterStat : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private int id;
    [SerializeField] private StatAttribute moveSpeed;

    // Stat Attribute Dictionary
    private Dictionary<AttributeType, StatAttribute> attributeDict = new Dictionary<AttributeType, StatAttribute>();

    public void InitStat(MonsterData data)
    {
        id = data.Id;
        moveSpeed = new StatAttribute(0, data.MoveSpeed, 0.1f);

        attributeDict.Clear();
        attributeDict.Add(AttributeType.MoveSpeed, moveSpeed);
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
    public Attribute MoveSpeed { get { return moveSpeed; } }
}
