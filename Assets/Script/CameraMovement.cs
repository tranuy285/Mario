using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Biến để tham chiếu đến Transform của nhân vật người chơi
    private Transform player;

    private void Awake()
    {
        // Tìm và gán Transform của nhân vật người chơi dựa trên thẻ "Player" với điều kiện chỉ có một nhân vật, nếu có 2 nhân vật trở nên thì lỗi
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        // Cập nhật vị trí của camera để theo dõi nhân vật người chơi
        if (player != null)
        {
            Vector3 cameraPosition = transform.position;
            //Chọn 1 trong 2 giá trị lớn hơn giữa vị trí hiện tại của camera và vị trí của nhân vật người chơi trên trục x, đảm bảo camera chỉ di chuyển về bên phải khi nhân vật di chuyển
            cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
            transform.position = cameraPosition;
        }
    }
}
