using System;
using UnityEngine;
using CasualGame.Enum;

// Unit 시트 데이터 컨테이너 클래스
// 필드 이름은 시트 1행과 같게 유지한다.
[Serializable]
public class UnitData
{
    [Header("유닛 ID")]
    public int Id;                                      // 고유 ID
    [Header("유닛 이름")]
    public string Name;                                 // 이름
    [Header("유닛 등급")]
    public Tier Tier;                                   // 등급
    [Header("유닛 공격력")]
    public float Attack;                                // 공격력
    [Header("초당 공격 횟수")]
    public float AttackSpeed;                           // 초당 공격 횟수
    [Header("유닛 사거리")]
    public float Range;                                 // 사거리
    [Header("유닛 프리팹")]
    public string Prefab;                               // 프리팹 이름
}
