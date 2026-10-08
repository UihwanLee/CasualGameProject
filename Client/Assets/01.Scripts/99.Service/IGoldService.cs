// 골드 보유량 관리
public interface IGoldService
{
    int Gold { get; }

    void Set(int gold);
    void Add(int amount);
}
