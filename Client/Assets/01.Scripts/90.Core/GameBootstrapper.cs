using UnityEngine;

// 게임 씬의 진입점
// 서비스를 의존 순서대로 생성해 ServiceLocator에 등록하고, 씬이 끝나면 한 번에 해제한다.
// 다른 스크립트의 Awake보다 먼저 실행되어야 하므로 실행 순서를 앞으로 당긴다.
[DefaultExecutionOrder(-1000)]
public class GameBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        InstallServices();
    }

    private void OnDestroy()
    {
        ServiceLocator.Clear();
    }

    /// <summary>
    /// 서비스 생성 및 등록 (생성 순서 = 의존 순서)
    /// </summary>
    private void InstallServices()
    {
        // 등록 순서: DataTable → Pool → Gold → MonsterSpawn → Wave → GameState → Board → SummonApi → Summon
    }
}
