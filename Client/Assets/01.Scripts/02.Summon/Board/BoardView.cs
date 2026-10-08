using UnityEngine;

// 보드 슬롯 위치를 씬에서 지정하는 컴포넌트 (씬마다 하나)
// 슬롯 상태와 배치 로직은 BoardService가 담당하고, 여기서는 위치만 제공한다.
public class BoardView : MonoBehaviour
{
    [Header("슬롯 위치")]
    [SerializeField] private Transform[] slots;

    public int SlotCount => slots.Length;

    public Vector3 GetSlotPosition(int slotIndex)
    {
        return slots[slotIndex].position;
    }
}
