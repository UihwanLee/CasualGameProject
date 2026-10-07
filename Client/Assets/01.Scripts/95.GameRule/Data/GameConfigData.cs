using System;
using UnityEngine;

// GameConfig 시트 데이터 컨테이너 클래스 (Key-Value)
// 필드 이름은 시트 1행과 같게 유지한다.
[Serializable]
public class GameConfigData
{
    [Header("설정 이름")]
    public string Key;                                  // 설정 이름
    [Header("값")]
    public float Value;                                 // 값
}
