using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Biến tham chiếu đến SpriteRenderer của nhân vật
    public Sprite deadSprite; // Biến lưu trữ sprite khi nhân vật chết


    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10; // Đặt thứ tự sắp xếp cao để hiển thị trên các đối tượng khác
        if (deadSprite != null) spriteRenderer.sprite = deadSprite;
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false; // Vô hiệu hóa tất cả các collider để tránh va chạm
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; // Vô hiệu hóa mô phỏng vật lý

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();
        if (playerMovement != null) playerMovement.enabled = false; // Vô hiệu hóa script di chuyển của nhân vật
        if (entityMovement != null) entityMovement.enabled = false; // Vô hiệu hóa script di chuyển của thực thể
    }
    
    private IEnumerator Animate()
    {
        float elapsed = 0f; // Thời gian đã trôi qua
        float duration = 3f; // Thời gian hoạt ảnh

        float jumpVelocity = 10f; // Vận tốc nhảy ban đầu
        float gravity = -36f; // Gia tốc trọng lực

        Vector3 velocity = Vector3.up * jumpVelocity; // Vận tốc ban đầu theo phương thẳng đứng
        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime; // Cập nhật vị trí dựa trên vận tốc
            velocity.y += gravity * Time.deltaTime; // Cập nhật vận tốc theo trọng lực
            elapsed += Time.deltaTime; // Cập nhật thời gian đã trôi qua
            yield return null; // Chờ đến khung hình tiếp theo
        }
    }
}
