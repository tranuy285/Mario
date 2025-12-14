using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite; //sprite dạng vỏ rùa của Koopa
    public float shellMoveSpeed = 12f; //tốc độ di chuyển của vỏ rùa
    private bool isShelled; //biến kiểm tra trạng thái vỏ rùa
    private bool isMovingShell; //biến kiểm tra trạng thái vỏ rùa di chuyển

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isShelled && collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();

            if(player.starpower)
            {
                Hit();
            }
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                ShellMode(); // Gọi hàm ShellMode để xử lý Koopa vào trạng thái vỏ rùa sau khi nhân vật nhảy lên đầu 
            }
            else
            {

                player.Hit();
            }
        }
    }

    private void ShellMode()
    {
        isShelled = true;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isShelled && other.CompareTag("Player"))
        {
            if (!isMovingShell)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0.0f);
                PushShell(direction);
            }
            else
            {   
                // Nếu nhân vật nhảy dẫm lên vỏ rùa đang di chuyển thì vỏ rùa sẽ dừng lại 
                if (other.transform.DotTest(transform, Vector2.down))
                {
                    StopShell();
                }
                else
                {
                    PlayerStatus player = other.GetComponent<PlayerStatus>();

                    if(player.starpower)
                    {
                        Hit();
                    }
                    else
                    {
                        player.Hit();
                    }
                }
            }
        }
        else if (!isShelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
        //Xử lý nếu 2 vỏ rùa bị đẩy bởi người chơi chạm vào nhau thì cùng biến mất
        else if (isShelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
            other.GetComponent<Koopa>().Hit();
        }
    }

    private void PushShell(Vector2 direction)
    {
        isMovingShell = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        EntityMovement entityMovement = GetComponent<EntityMovement>();
        entityMovement.direction = direction.normalized;
        entityMovement.moveSpeed = shellMoveSpeed;
        entityMovement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        
        // Thêm 2 coins khi kill Koopa
        GameManager.Instance?.AddCoin();
        GameManager.Instance?.AddCoin();
        
        Destroy(gameObject, 3f);
    }

    private void DestroyKoopa()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        if (isMovingShell)
            Invoke(nameof(DestroyKoopa), 3.0f);
    }

    private void OnBecameVisible()
    {
        if (isMovingShell)
            CancelInvoke(nameof(DestroyKoopa));
    }
    
    //Dừng sự di chuyển của vỏ
    private void StopShell()
    {
        isMovingShell = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = Vector2.zero;
        movement.moveSpeed = 0f;
        movement.enabled = false;
    }
}
