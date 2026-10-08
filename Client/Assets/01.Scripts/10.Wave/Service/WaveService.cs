using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

// 웨이브 진행 서비스
// 웨이브마다 정해진 수만큼 몬스터를 스폰하고, 제한 시간이 지나면 다음 웨이브로 넘어간다.
// 스폰할 때마다 웨이브의 몬스터 목록에서 가중치로 한 종을 뽑는다.
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

            if (!IsValidWave(wave))
                continue;

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
            monsterSpawn.Spawn(PickMonsterId(wave));

            if (wave.SpawnInterval > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(wave.SpawnInterval), cancellationToken: token);
        }
    }

    /// <summary>
    /// 웨이브 데이터 검사 (몬스터 목록이 비었으면 스폰하지 않고, 가중치 길이가 다르면 균등으로 처리)
    /// </summary>
    private bool IsValidWave(WaveData wave)
    {
        if (wave.MonsterIds == null || wave.MonsterIds.Length == 0)
        {
            Debug.LogError($"웨이브 {wave.Id}의 MonsterIds가 비어 있습니다.");
            return false;
        }

        if (wave.SpawnWeights != null && wave.SpawnWeights.Length > 0 && wave.SpawnWeights.Length != wave.MonsterIds.Length)
            Debug.LogWarning($"웨이브 {wave.Id}의 SpawnWeights 길이가 MonsterIds와 달라 균등 확률로 스폰합니다.");

        return true;
    }

    /// <summary>
    /// 가중치에 따라 스폰할 몬스터 한 종 추첨 (가중치가 없거나 잘못되면 균등)
    /// </summary>
    private int PickMonsterId(WaveData wave)
    {
        int[] ids = wave.MonsterIds;
        float[] weights = wave.SpawnWeights;

        float total = 0f;
        if (weights != null && weights.Length == ids.Length)
        {
            for (int i = 0; i < weights.Length; i++)
                total += Mathf.Max(weights[i], 0f);
        }

        if (total <= 0f)
            return ids[UnityEngine.Random.Range(0, ids.Length)];

        float roll = UnityEngine.Random.Range(0f, total);
        for (int i = 0; i < ids.Length; i++)
        {
            roll -= Mathf.Max(weights[i], 0f);
            if (roll < 0f)
                return ids[i];
        }

        return ids[ids.Length - 1];
    }
}
