using System.Collections.Generic;
using UnityEngine;

// 게임 씬의 진입점
// 서비스를 의존 순서대로 생성해 ServiceLocator에 등록하고, 씬이 끝나면 한 번에 해제한다.
// 다른 스크립트의 Awake보다 먼저 실행되어야 하므로 실행 순서를 앞으로 당긴다.
[DefaultExecutionOrder(-1000)]
public class GameBootstrapper : MonoBehaviour
{
    [Header("임시 데이터 (시트 연동 전)")]
    [SerializeField] private List<UnitData> unitDataList = new List<UnitData>();
    [SerializeField] private List<MonsterData> monsterDataList = new List<MonsterData>();
    [SerializeField] private List<WaveData> waveDataList = new List<WaveData>();
    [SerializeField] private List<SummonRateData> summonRateDataList = new List<SummonRateData>();

    [Header("몬스터")]
    [SerializeField] private MonsterPath monsterPath;
    [SerializeField] private Monster[] monsterPrefabs;
    [SerializeField] private int monsterPoolInitialSize = 20;

    [Header("유닛")]
    [SerializeField] private BoardView boardView;
    [SerializeField] private Unit[] unitPrefabs;
    [SerializeField] private int unitPoolInitialSize = 5;

    private void Awake()
    {
        PrefabPool<Monster> monsterPool = new PrefabPool<Monster>(transform);
        PrefabPool<Unit> unitPool = new PrefabPool<Unit>(transform);

        InstallServices(monsterPool, unitPool);

        // 엔티티의 Awake가 서비스를 Resolve하므로, 프리팹 미리 생성은 모든 서비스를 등록한 뒤에 한다
        RegisterPrefabs(monsterPool, monsterPrefabs, monsterPoolInitialSize);
        RegisterPrefabs(unitPool, unitPrefabs, unitPoolInitialSize);
    }

    private void OnDestroy()
    {
        ServiceLocator.Clear();
    }

    /// <summary>
    /// 서비스 생성 및 등록 (생성 순서 = 의존 순서)
    /// </summary>
    private void InstallServices(PrefabPool<Monster> monsterPool, PrefabPool<Unit> unitPool)
    {
        DataTableService dataTable = new DataTableService(unitDataList, monsterDataList, waveDataList, summonRateDataList);
        ServiceLocator.Bind<IDataTableService>(dataTable);

        ServiceLocator.Bind<IPrefabPool<Monster>>(monsterPool);
        ServiceLocator.Bind<IPrefabPool<Unit>>(unitPool);

        GoldService gold = new GoldService();
        ServiceLocator.Bind<IGoldService>(gold);

        MonsterSpawnService monsterSpawn = new MonsterSpawnService(dataTable, monsterPool, monsterPath);
        ServiceLocator.Bind<IMonsterSpawnService>(monsterSpawn);

        WaveService wave = new WaveService(dataTable, monsterSpawn);
        ServiceLocator.Bind<IWaveService>(wave);

        GameStateService gameState = new GameStateService(gold, wave);
        ServiceLocator.Bind<IGameStateService>(gameState);

        BoardService board = new BoardService(dataTable, unitPool, boardView);
        ServiceLocator.Bind<IBoardService>(board);

        LocalSummonApi summonApi = new LocalSummonApi(board, gold, dataTable);
        ServiceLocator.Bind<ISummonApi>(summonApi);

        SummonService summon = new SummonService(summonApi, board, gold, gameState);
        ServiceLocator.Bind<ISummonService>(summon);
    }

    /// <summary>
    /// 프리팹 이름을 키로 풀에 등록 (데이터의 Prefab 값과 프리팹 이름이 같아야 한다)
    /// </summary>
    private void RegisterPrefabs<T>(PrefabPool<T> pool, T[] prefabs, int initialSize) where T : Component
    {
        foreach (T prefab in prefabs)
            pool.Register(prefab.name, prefab, initialSize);
    }
}
