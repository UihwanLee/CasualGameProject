// 소환/합성 요청과 결과 반영
public interface ISummonService
{
    bool IsRequesting { get; }

    void RequestSummon();
    void RequestMerge(int slotIndex);
}
