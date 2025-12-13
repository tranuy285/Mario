using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Biến để tham chiếu đến Transform của nhân vật người chơi
    private Transform player;

    public float height = 6.5f; // Chiều cao cố định của camera bên trên mặt đất
    public float undergroundHeight = -10f; // Chiều cao của camera khi ở dưới lòng đất

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

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }
}
