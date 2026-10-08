using System;
using UnityEngine;

// Wave 시트 데이터 컨테이너 클래스
// 필드 이름은 시트 1행과 같게 유지한다.
// 시트에서는 MonsterIds / SpawnWeights를 "1|2" 형식 문자열로 쓰고, DataTool이 배열로 변환한다.
[Serializable]
public class WaveData
{
    [Header("웨이브 번호")]
    public int Id;                                      // 웨이브 번호
    [Header("스폰 몬스터 ID 목록")]
    public int[] MonsterIds;                            // 이 웨이브에 나오는 몬스터 ID 목록
    [Header("몬스터별 스폰 가중치 (비우면 균등)")]
    public float[] SpawnWeights;                        // MonsterIds와 같은 순서, 같은 길이
    [Header("스폰 수")]
    public int Count;                                   // 스폰 수
    [Header("스폰 간격(초)")]
    public float SpawnInterval;                         // 스폰 간격(초)
    [Header("제한 시간(초)")]
    public float TimeLimit;                             // 제한 시간(초)
}
