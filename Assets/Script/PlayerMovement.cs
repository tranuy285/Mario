using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;     // Biến để tham chiếu đến Camera
    private new Rigidbody2D rigidbody; //Khai báo biến rigidbody2D để điều khiển vật lý của nhân vật, thêm new để tránh cảnh báo trùng tên
    private Vector2 velocity; //Biến lưu trữ thông tin di chuyển
    private float inputAxis; //Biến lưu trữ giá trị đầu vào từ bàn phím

    public float movementSpeed = 10f; //Tốc độ di chuyển của nhân vật
    public float maxJumpHeight = 5f; //Chiều cao nhảy tối đa
    public float maxJumpTime = 1f; //Thời gian tối đa để đạt chiều cao nhảy


    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f); //Công thức tính lực nhảy dựa trên chiều cao và thời gian nhảy
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2f); //Công thức tính trọng lực dựa trên chiều cao và bình phương thời gian nhảy
    /*
    Khai báo public property vì sẽ cần get trạng thái isGrounded và isJumping cho các lớp khác, nhưng set trạng thái thì chỉ trong class này nên set là private, đảm bảo tính đóng gói
    */
    public bool isGrounded { get; private set; } //Biến kiểm tra trạng thái tiếp đất
    public bool isJumping { get; private set; } //biến kiểm tra trạng thái nhảy
    public bool isRunning => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f; //biến kiểm tra trạng thái chạy, kiểm tra hoặc điều kiện vận tốc x hoặc giá trị đầu vào bàn phím > 0.25f thì là đang chạy
    public bool isTurning => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f); //biến kiểm tra trạng thái quay, nếu giá trị đầu vào bàn phím khác dấu với vận tốc x thì là đang quay
    
    private void Awake()
    {
        //Lấy thành phần Rigidbody2D gắn trên cùng GameObject
        rigidbody = GetComponent<Rigidbody2D>();
    
        camera = Camera.main; //Lấy tham chiếu đến Camera chính trong cảnh
    }
    private void Update()
    {
        HorizontalMovement();
        isGrounded = rigidbody.Raycast(Vector2.down); //Cập nhật trạng thái isGrounded bằng cách kiểm tra va chạm xuống dưới
        if (isGrounded)
        {
            GroundedMovement();
        }
        ApplyGravity();
    }

    //Hàm di chuyển ngang cho nhân vật
    private void HorizontalMovement()
    {
        //Lấy giá trị đầu vào từ bàn phím
        inputAxis = Input.GetAxisRaw("Horizontal");
        //Tính toán vận tốc di chuyển ngang
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movementSpeed, movementSpeed * Time.deltaTime);
        //Kiểm tra va chạm với tường nếu có va chạm thì đặt vận tốc x về 0
        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }
        
        // Xử lý quay hướng nhân vật dựa trên vận tốc x
        if (velocity.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); //Quay nhân vật về bên phải
        }
        else if (velocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); //Quay nhân vật về bên trái
        }
    }

    //Hàm xử lý logic chuyển đổi giữa hai trạng thái nhảy và tiếp đất
    private void GroundedMovement()
    {
        /* 
        Khi nhân vật tiếp đất, đặt vận tốc y về 0 để tránh việc tích tụ vận tốc rơi khi chạm đất
        Vì khi di chuyển dưới đất, vận tốc y có thể âm do trọng lực, nếu không đặt về 0 thì khi nhảy sẽ bị ảnh hưởng bởi vận tốc âm
        */
        velocity.y = Mathf.Max(velocity.y, 0f); 
        /*
        //Cập nhật trạng thái isJumping dựa trên vận tốc y
        Nếu set isJumping = false thì khi nhân vật chạm đất sẽ không thể nhảy ngay được vì isJumping luôn là false, nên phải kiểm tra vận tốc y để xác định trạng thái nhảy 
        */
        isJumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce; //Áp dụng lực nhảy
            isJumping = true; //Cập nhật trạng thái isJumping
        }
        
    }
    
    //Hàm xử lý trọng lực
    private void ApplyGravity()
    {
        bool falling = velocity.y <= 0f || !Input.GetButton("Jump"); //Kiểm tra nếu nhân vật đang rơi hoặc không giữ nút nhảy
        float gravityMultiplier = falling ? 2f : 1f; //Tăng trọng lực khi rơi để cảm giác rơi nhanh hơn nếu không giữ nút nhảy
        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity/2f); //Đảm bảo vận tốc rơi không trở nên quá lớn hoặc quá nhỏ, tránh việc rơi quá nhanh
    }

    //Hàm FixedUpdate để xử lý vật lý vì FixedUpdate chạy với tần số cố định giúp việc tính toán vật lý ổn định hơn
    private void FixedUpdate()
    {
        //Cập nhật vị trí của nhân vật 
        Vector2 position = rigidbody.position;
        //Cập nhật vị trí mới dựa trên vận tốc
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float minX = leftEdge.x + 0.5f;
        float maxX = rightEdge.x - 0.5f;
        position.x = Mathf.Clamp(position.x, minX, maxX);

        // Nếu nhân vật chạm vào biên trái, set velocity.x = 0
        if (position.x <= minX && velocity.x < 0)
        {
            velocity.x = 0f;
        }

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                // Nhân vật chạm kẻ địch từ trên xuống
                velocity.y = jumpForce * 0.5f; // Nhân vật bật lên sau khi nhảy lên đầu kẻ địch
                isJumping = true; // Cập nhật trạng thái nhảy
            }
        }
    
        /*
        Kiểm tra nếu va chạm không phải với vật phẩm PowerUp thì mới xử lý va chạm 
        Tránh việc va chạm với PowerUp làm văng PowerUp đi chứ không phải nhặt được
        */
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f; //Đặt vận tốc y về 0 khi va chạm từ dưới lên
            }
        }

        
    }
}
