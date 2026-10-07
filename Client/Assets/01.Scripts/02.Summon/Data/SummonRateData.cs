using System;
using UnityEngine;
using CasualGame.Enum;

// SummonRate 시트 데이터 컨테이너 클래스
// 필드 이름은 시트 1행과 같게 유지한다.
[Serializable]
public class SummonRateData
{
    [Header("등급")]
    public Tier Tier;                                   // 등급
    [Header("소환 확률(%)")]
    public float Rate;                                  // 소환 확률(%). 합계 100
}
