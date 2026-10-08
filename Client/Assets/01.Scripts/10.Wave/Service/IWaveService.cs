// 웨이브 진행
public interface IWaveService
{
    int CurrentWave { get; }

    void StartWave();
    void StopWave();
}
