using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using CasualGame.Enum;

// 소환/합성 요청을 보내고 결과를 보드와 골드에 반영하는 서비스
// 결과는 ISummonApi가 결정하고, 응답을 기다리는 동안에는 다음 요청을 받지 않는다.
public class SummonService : ISummonService, IDisposable
{
    private readonly ISummonApi api;
    private readonly IBoardService board;
    private readonly IGoldService gold;
    private readonly IGameStateService gameState;

    private readonly CancellationTokenSource cts = new CancellationTokenSource();
    private bool isRequesting;

    public bool IsRequesting => isRequesting;

    public SummonService(ISummonApi api, IBoardService board, IGoldService gold, IGameStateService gameState)
    {
        this.api = api;
        this.board = board;
        this.gold = gold;
        this.gameState = gameState;
    }

    public void Dispose()
    {
        cts.Cancel();
        cts.Dispose();
    }

    /// <summary>
    /// 소환 버튼에서 호출
    /// </summary>
    public void RequestSummon()
    {
        if (!CanRequest())
            return;

        SummonAsync(cts.Token).Forget();
    }

    /// <summary>
    /// 유닛 선택 후 합성 버튼에서 호출
    /// </summary>
    /// <param name="slotIndex">선택한 유닛의 슬롯</param>
    public void RequestMerge(int slotIndex)
    {
        if (!CanRequest())
            return;

        MergeAsync(slotIndex, cts.Token).Forget();
    }

    private bool CanRequest()
    {
        return !isRequesting && gameState.State == GameState.RUNNING;
    }

    private async UniTaskVoid SummonAsync(CancellationToken token)
    {
        isRequesting = true;
        try
        {
            SummonResult result = await api.SummonAsync(token);

            if (result.Code != ErrorCode.Ok)
            {
                EventBus.OnRequestFailed?.Invoke(result.Code);
                return;
            }

            gold.Set(result.Gold);
            EventBus.OnSummonCostChanged?.Invoke(result.NextCost);
            board.Place(result.SlotIndex, result.UnitId);
        }
        finally
        {
            isRequesting = false;
        }
    }

    private async UniTaskVoid MergeAsync(int slotIndex, CancellationToken token)
    {
        isRequesting = true;
        try
        {
            MergeResult result = await api.MergeAsync(slotIndex, token);

            if (result.Code != ErrorCode.Ok)
            {
                EventBus.OnRequestFailed?.Invoke(result.Code);
                return;
            }

            foreach (int consumed in result.ConsumedSlots)
                board.Remove(consumed);

            board.Place(result.SlotIndex, result.UnitId);
        }
        finally
        {
            isRequesting = false;
        }
    }
}
