using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 direction = Vector2.left;
    private new Rigidbody2D rigidbody;
    private Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

     private void OnBecameInvisible()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Shell"))
            return;
        
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.Sleep();
    }

   

    private void FixedUpdate()
    {
        velocity.x = direction.x * moveSpeed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime; // Áp dụng trọng lực có sẵn của Unity, đảm bảo gia tốc của trọng lực tăng dần theo thời gian

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime); // Entity di chuyển theo thời gian

        // Kiểm tra va chạm phía trước, nếu có thì đổi hướng di chuyển
        if (rigidbody.Raycast(direction))
        {
            direction = -direction;
        }

        if (rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f); // Tránh tích tụ vận tốc rơi khi còn trên mặt đất
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Shell")){
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            direction = -direction;
        } 
    }
}
