using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer; //Tham chiếu tới sprite dạng nhỏ của Mario
    public PlayerSpriteRenderer bigRenderer; //Tham chiếu tới sprite dạng lớn của Mario
    private PlayerSpriteRenderer activeRenderer; //Tham chiếu tới sprite hiện tại đang được hiển thị

    private DeathAnimation deathAnimation; //Tham chiếu tới script DeathAnimation để kích hoạt hoạt ảnh chết
    private CapsuleCollider2D capsuleCollider; //Tham chiếu tới collider của nhân vật

    public bool isBig => bigRenderer.enabled; //Kiểm tra trạng thái lớn hay nhỏ của nhân vật dựa trên việc sprite lớn có được kích hoạt hay không
    public bool isSmall => smallRenderer.enabled; //Kiểm tra trạng thái nhỏ của nhân vật dựa trên việc sprite nhỏ có được kích hoạt hay không
    public bool isDead => deathAnimation.enabled; //Kiểm tra trạng thái chết của nhân vật dựa trên việc script DeathAnimation có được kích hoạt hay không
    public bool starpower { get; private set; } //Trạng thái bất tử của nhân vật

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if(!isDead && !starpower)
        {
            if (isBig)
            {
                Shrink(); 
            } else if (isSmall)
            {
                Death();
            }
        }
        
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.LevelReset(3f); // Gọi hàm xử lý khi người chơi chết với độ trễ 3 giây


    }

    // Hàm phóng to mario
    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    // Hàm thu nhỏ mario 
    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !bigRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower(float duration = 10f)
    {
        StartCoroutine(StarpowerAnimation(duration));
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        starpower = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }
}
