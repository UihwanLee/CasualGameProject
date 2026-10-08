using System.Threading;
using Cysharp.Threading.Tasks;

// 소환/합성 결과를 결정하는 쪽의 인터페이스
// 클라이언트는 결과만 받아서 보드에 반영하므로, 구현체를 로컬/서버로 바꿔 끼울 수 있다.
public interface ISummonApi
{
    UniTask<SummonResult> SummonAsync(CancellationToken token);
    UniTask<MergeResult> MergeAsync(int slotIndex, CancellationToken token);
}
