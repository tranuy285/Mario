using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float aggroRange = 5f;
    [SerializeField] private float chaseSpeed = 2f;
    
    // Layer chỉ chứa các vật cản (wall, ground, platform...)
    [SerializeField] private LayerMask obstacleLayer;

    private EntityMovement entityMove;
    private float originalSpeed;
    private bool isChasing = false;
    private Transform playerTransform;

    private void Awake()
    {
        entityMove = GetComponent<EntityMovement>();
        if (entityMove == null)
        {
            Debug.LogError("EnemyChase yêu cầu component EntityMovement!", this);
            enabled = false;
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
        else
        {
            Debug.LogWarning("Không tìm thấy Player với tag 'Player'!", this);
            enabled = false;
        }
    }

    private void Start()
    {
        originalSpeed = entityMove.moveSpeed;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Bước 1: Kiểm tra khoảng cách trước (tối ưu)
        if (distanceToPlayer < aggroRange)
        {
            // Bước 2: Kiểm tra Line-of-Sight (có thấy player không, có bị tường chắn không)
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, aggroRange, obstacleLayer);

            bool canSeePlayer = false;

            if (hit.collider == null)
            {
                // Không va chạm gì → có thể thấy player
                canSeePlayer = true;
            }
            else if (hit.collider.CompareTag("Player") || hit.transform == playerTransform)
            {
                // Va chạm đầu tiên là Player → vẫn thấy được (trường hợp player đứng sát tường)
                canSeePlayer = true;
            }

            if (canSeePlayer)
            {
                // Có thể thấy player → Chase!
                if (!isChasing)
                {
                    isChasing = true;
                    Debug.Log($"[Chase] {gameObject.name} PHÁT HIỆN Player và đuổi theo!");
                    entityMove.moveSpeed = chaseSpeed;
                }

                // Cập nhật hướng real-time
                entityMove.direction = playerTransform.position.x > transform.position.x ? Vector2.right : Vector2.left;
            }
            else
            {
                // Bị tường chắn → mất tầm nhìn
                StopChasing();
            }
        }
        else
        {
            // Ra khỏi phạm vi
            StopChasing();
        }
    }

    private void StopChasing()
    {
        if (isChasing)
        {
            isChasing = false;
            Debug.Log($"[Chase] {gameObject.name} mất mục tiêu (ra khỏi tầm hoặc bị chắn).");
            entityMove.moveSpeed = originalSpeed;
            // Giữ hướng cuối cùng để patrol tiếp tục mượt
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = isChasing ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRange);

            // Vẽ đường line-of-sight để debug
            Vector2 dir = (playerTransform.position - transform.position);
            float dist = Mathf.Min(dir.magnitude, aggroRange);
            Gizmos.color = isChasing ? Color.green : Color.gray;
            Gizmos.DrawRay(transform.position, dir.normalized * dist);
        }
    }
#endif
}