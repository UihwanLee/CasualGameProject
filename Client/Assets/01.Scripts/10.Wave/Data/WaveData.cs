using System;
using UnityEngine;

// Wave 시트 데이터 컨테이너 클래스
// 필드 이름은 시트 1행과 같게 유지한다.
[Serializable]
public class WaveData
{
    [Header("웨이브 번호")]
    public int Id;                                      // 웨이브 번호
    [Header("스폰 몬스터 ID")]
    public int MonsterId;                               // 스폰 몬스터 ID
    [Header("스폰 수")]
    public int Count;                                   // 스폰 수
    [Header("스폰 간격(초)")]
    public float SpawnInterval;                         // 스폰 간격(초)
    [Header("제한 시간(초)")]
    public float TimeLimit;                             // 제한 시간(초)
}
