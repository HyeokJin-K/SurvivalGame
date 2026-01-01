using UnityEngine;

public class MiniMapCameraMove : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float height = 10f;

    private void LateUpdate()
    {
        transform.position = player.position + new Vector3(0f, height, 0f);
    }
}
