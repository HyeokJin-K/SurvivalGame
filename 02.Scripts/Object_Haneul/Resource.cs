using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGIve; // 드랍될 아이템
    public int quantityPerHit = 1; // 드랍 개수
    public int capacity; // 총 타격 횟수

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i<quantityPerHit; i++)
        {
            if (capacity <= 0) break; // 타격 가능 횟수 0일 경우

            capacity -= 1; // 1타격 = 1 드랍
            Instantiate(itemToGIve.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        if ( capacity <=0) // 채집 완료
        {
            Destroy(gameObject);
        }
    }
}
