using UnityEngine;

// 게임 내 오브젝트들의 Data에 접근할 수 있는 전역 클래스
// Data는 Excel->Json으로 변환된 데이터 구조를 이용하여 저장하고 있다.
// Dictionary 자료구조를 통해 Data를 가지고 있으며 고유 ID로 Key를 분리한다.
// TODO(M2): DataTool로 생성한 데이터 클래스와 JSON 로드 연결
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAllData();
    }

    /// <summary>
    /// 모든 데이터 테이블 로드
    /// </summary>
    private void LoadAllData()
    {
    }
}
