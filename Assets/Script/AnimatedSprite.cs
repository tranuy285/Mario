using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites; // Mảng chứa các khung hình của hoạt ảnh
    public float framerate = 1f / 6f; // Tốc độ khung hình (mặc định 6 khung hình mỗi giây)

    private SpriteRenderer spriteRenderer; // Tham chiếu đến thành phần SpriteRenderer
    private int currentFrame; // Chỉ số khung hình hiện tại

    private void Awake()
    {
        // Lấy thành phần SpriteRenderer gắn trên cùng GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Bắt đầu phát hoạt ảnh khi đối tượng được kích hoạt
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    private void OnDisable()
    {
        // Dừng phát hoạt ảnh khi đối tượng bị vô hiệu hóa
        CancelInvoke();
    }

    private void Animate()
    {
        currentFrame++;
        if (currentFrame >= sprites.Length)
        {
            currentFrame = 0; // Quay lại khung hình đầu tiên nếu đã đến cuối mảng
        }

        //Kiểm tra để tránh lỗi truy cập mảng ngoài phạm vi
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[currentFrame]; // Cập nhật sprite hiện tại trong khung hình
        }
    }
}
