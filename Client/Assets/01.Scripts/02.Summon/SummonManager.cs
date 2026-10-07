using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CasualGame.Enum;

// 소환/합성 요청을 보내고 결과를 보드와 골드에 반영하는 매니저 (씬마다 하나)
// 응답을 기다리는 동안에는 다음 요청을 받지 않는다.
public class SummonManager : MonoBehaviour
{
    public static SummonManager Instance { get; private set; }

    [SerializeField] private Board board;

    private ISummonService service;
    private bool isRequesting;

    public bool IsRequesting => isRequesting;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        service = new LocalSummonService(board);
    }

    /// <summary>
    /// 소환 버튼에서 호출
    /// </summary>
    public void RequestSummon()
    {
        if (!CanRequest())
            return;

        SummonAsync(destroyCancellationToken).Forget();
    }

    /// <summary>
    /// 유닛 선택 후 합성 버튼에서 호출
    /// </summary>
    /// <param name="slotIndex">선택한 유닛의 슬롯</param>
    public void RequestMerge(int slotIndex)
    {
        if (!CanRequest())
            return;

        MergeAsync(slotIndex, destroyCancellationToken).Forget();
    }

    private bool CanRequest()
    {
        return !isRequesting && GameManager.Instance.State == GameState.RUNNING;
    }

    private async UniTaskVoid SummonAsync(CancellationToken token)
    {
        isRequesting = true;
        try
        {
            SummonResult result = await service.SummonAsync(token);

            if (result.Code != ErrorCode.Ok)
            {
                EventBus.OnRequestFailed?.Invoke(result.Code);
                return;
            }

            GoldManager.Instance.Set(result.Gold);
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
            MergeResult result = await service.MergeAsync(slotIndex, token);

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
