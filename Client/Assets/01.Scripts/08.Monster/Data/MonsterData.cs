using System;
using UnityEngine;

// Monster 시트 데이터 컨테이너 클래스
// 필드 이름은 시트 1행과 같게 유지한다.
[Serializable]
public class MonsterData
{
    [Header("몬스터 ID")]
    public int Id;                                      // 고유 ID
    [Header("몬스터 이름")]
    public string Name;                                 // 이름
    [Header("몬스터 체력")]
    public float Hp;                                    // 체력
    [Header("몬스터 이동속도")]
    public float MoveSpeed;                             // 이동 속도
    [Header("본진 공격력")]
    public float Attack;                                // 본진 공격력
    [Header("초당 공격 횟수")]
    public float AttackSpeed;                           // 초당 공격 횟수
    [Header("공격 사거리")]
    public float Range;                                 // 본진 반지름 바깥으로부터의 공격 사거리
    [Header("처치 시 골드")]
    public int Gold;                                    // 처치 시 골드
    [Header("보스 여부")]
    public bool IsBoss;                                 // 보스 여부
    [Header("몬스터 프리팹")]
    public string Prefab;                               // 프리팹 이름
}
