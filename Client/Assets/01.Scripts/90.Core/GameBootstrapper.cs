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
        DataTableService dataTable = new DataTableService(unitDataList, monsterDataList, waveDataList, summonRateDataList);
        ServiceLocator.Bind<IDataTableService>(dataTable);

        PoolService pool = new PoolService(transform);
        ServiceLocator.Bind<IPoolService>(pool);

        GoldService gold = new GoldService();
        ServiceLocator.Bind<IGoldService>(gold);

        MonsterSpawnService monsterSpawn = new MonsterSpawnService(dataTable, pool, monsterPath, monsterPrefabs, monsterPoolInitialSize);
        ServiceLocator.Bind<IMonsterSpawnService>(monsterSpawn);

        WaveService wave = new WaveService(dataTable, monsterSpawn);
        ServiceLocator.Bind<IWaveService>(wave);

        GameStateService gameState = new GameStateService(gold, wave);
        ServiceLocator.Bind<IGameStateService>(gameState);

        BoardService board = new BoardService(dataTable, pool, boardView, unitPrefabs, unitPoolInitialSize);
        ServiceLocator.Bind<IBoardService>(board);

        LocalSummonApi summonApi = new LocalSummonApi(board, gold, dataTable);
        ServiceLocator.Bind<ISummonApi>(summonApi);

        SummonService summon = new SummonService(summonApi, board, gold, gameState);
        ServiceLocator.Bind<ISummonService>(summon);
    }
}
