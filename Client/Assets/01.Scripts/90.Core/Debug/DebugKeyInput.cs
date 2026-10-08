using UnityEngine;
using UnityEngine.InputSystem;
using CasualGame.Enum;

// 개발용 키 입력 (UI 없이 코어 루프를 테스트하기 위한 용도)
// 에디터와 개발 빌드에서만 동작한다.
//   Space : 게임 시작
//   S     : 소환
//   1 ~ 9 : 해당 슬롯(0 ~ 8) 유닛 합성
public class DebugKeyInput : MonoBehaviour
{
    private static readonly Key[] MERGE_KEYS =
    {
        Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5,
        Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9,
    };

    private IGameStateService gameState;
    private ISummonService summon;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            enabled = false;
            return;
        }

        gameState = ServiceLocator.Resolve<IGameStateService>();
        summon = ServiceLocator.Resolve<ISummonService>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        // 재시작 흐름이 없으므로 준비 상태에서만 시작한다
        if (keyboard.spaceKey.wasPressedThisFrame && gameState.State == GameState.READY)
            gameState.StartGame();

        if (keyboard.sKey.wasPressedThisFrame)
            summon.RequestSummon();

        for (int i = 0; i < MERGE_KEYS.Length; i++)
        {
            if (keyboard[MERGE_KEYS[i]].wasPressedThisFrame)
                summon.RequestMerge(i);
        }
    }
}
