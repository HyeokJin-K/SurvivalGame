using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data; // 상호작용 기능


    public void OnInteract()
    {
        // 캐릭터.Instance.Player.itemData = data; // 데이터 넘기기
        // 캐릭터.Instance.Player.addItem?.Invoke(); // 실행
        Destroy(gameObject); // 아이템 획득 시 제거
    }
}

public interface IInteractable
{
    public void OnInteract(); // 상호작용 시
}
