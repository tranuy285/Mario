using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    private PlayerMovement playerMovement;

    public Sprite idle;
    public Sprite turning;
    public Sprite jumping;
    public AnimatedSprite running;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        running.enabled = false;
    }
    
    private void LateUpdate()
    {
        running.enabled = playerMovement.isRunning;
        if (playerMovement.isJumping)
        {
            spriteRenderer.sprite = jumping;
        }
        else if (playerMovement.isTurning)
        {
            spriteRenderer.sprite = turning;
        }
        else if (!playerMovement.isRunning)
        {
            spriteRenderer.sprite = idle;
        }
    }
}
