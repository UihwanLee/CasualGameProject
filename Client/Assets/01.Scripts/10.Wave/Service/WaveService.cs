using System;
using System.Threading;
using Cysharp.Threading.Tasks;

// 웨이브 진행 서비스
// 웨이브마다 정해진 수만큼 몬스터를 스폰하고, 제한 시간이 지나면 다음 웨이브로 넘어간다.
public class WaveService : IWaveService, IDisposable
{
    private readonly IDataTableService dataTable;
    private readonly IMonsterSpawnService monsterSpawn;

    private CancellationTokenSource cts;

    public int CurrentWave { get; private set; }

    public WaveService(IDataTableService dataTable, IMonsterSpawnService monsterSpawn)
    {
        this.dataTable = dataTable;
        this.monsterSpawn = monsterSpawn;
    }

    public void Dispose()
    {
        StopWave();
    }

    /// <summary>
    /// 첫 웨이브부터 진행 시작
    /// </summary>
    public void StartWave()
    {
        StopWave();

        cts = new CancellationTokenSource();
        RunWavesAsync(cts.Token).Forget();
    }

    /// <summary>
    /// 웨이브 진행 중단
    /// </summary>
    public void StopWave()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }

    private async UniTaskVoid RunWavesAsync(CancellationToken token)
    {
        foreach (WaveData wave in dataTable.WaveList)
        {
            CurrentWave = wave.Id;
            EventBus.OnWaveStart?.Invoke(wave.Id);

            // 스폰과 제한 시간은 동시에 흐른다
            await UniTask.WhenAll(
                SpawnWaveAsync(wave, token),
                UniTask.Delay(TimeSpan.FromSeconds(wave.TimeLimit), cancellationToken: token));
        }

        // TODO: 마지막 웨이브 이후 처리 (클리어 / 무한 웨이브)
    }

    private async UniTask SpawnWaveAsync(WaveData wave, CancellationToken token)
    {
        for (int i = 0; i < wave.Count; i++)
        {
            monsterSpawn.Spawn(wave.MonsterId);

            if (wave.SpawnInterval > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(wave.SpawnInterval), cancellationToken: token);
        }
    }
}
