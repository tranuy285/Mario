using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite diedSprite; // Sprite dạng chết của Goomba

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();

            if(player.starpower)
            {
                Hit();
            }
            // Kiểm tra nếu nhân vật chạm Goomba từ trên xuống => nhảy lên đầu Goomba
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flat(); // Gọi hàm Flat để xử lý Goomba chết
            }
            else
            {
                // Xử lý khi nhân vật chạm Goomba từ 2 phía trái phải
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    // Hàm xử lý khi Goomba chết 
    private void Flat()
    {
        GetComponent<Collider2D>().enabled = false; // Vô hiệu hóa collider để tránh va chạm sau khi chết
        GetComponent<EntityMovement>().enabled = false; // Vô hiệu hóa script di chuyển
        GetComponent<AnimatedSprite>().enabled = false; // Vô hiệu hóa script AnimatedSprite để dừng hoạt ảnh
        GetComponent<SpriteRenderer>().sprite = diedSprite; // Thay đổi sprite thành sprite chết 
        
        // Thêm coin khi kill Goomba
        GameManager.Instance?.AddCoin();
        
        Destroy(gameObject, 0.5f); // Hủy đối tượng Goomba sau 0.5 giây
    }

    //Hàm xử lý khi Goomba va chạm với Koopa trong trạng thái vỏ rùa bị đẩy bởi Mario
    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        transform.eulerAngles = new Vector3(180.0f, 0.0f, 0.0f);
        GetComponent<DeathAnimation>().enabled = true;
        
        // Thêm coin khi kill Goomba bằng shell
        GameManager.Instance?.AddCoin();
        
        Destroy(gameObject, 3f);
    }
}
