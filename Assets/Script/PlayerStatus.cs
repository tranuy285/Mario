using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer; //Tham chiếu tới sprite dạng nhỏ của Mario
    public PlayerSpriteRenderer bigRenderer; //Tham chiếu tới sprite dạng lớn của Mario

    private DeathAnimation deathAnimation; //Tham chiếu tới script DeathAnimation để kích hoạt hoạt ảnh chết

    public bool isBig => bigRenderer.enabled; //Kiểm tra trạng thái lớn hay nhỏ của nhân vật dựa trên việc sprite lớn có được kích hoạt hay không
    public bool isSmall => smallRenderer.enabled; //Kiểm tra trạng thái nhỏ của nhân vật dựa trên việc sprite nhỏ có được kích hoạt hay không
    public bool isDead => deathAnimation.enabled; //Kiểm tra trạng thái chết của nhân vật dựa trên việc script DeathAnimation có được kích hoạt hay không

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if (isBig)
        {
           Shrink(); 
        } else if (isSmall)
        {
            Death();
        }
    }

    // Hàm thu nhỏ mario (chưa làm)
    public void Shrink()
    {

    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.LevelReset(3f); // Gọi hàm xử lý khi người chơi chết với độ trễ 3 giây


    }
}
