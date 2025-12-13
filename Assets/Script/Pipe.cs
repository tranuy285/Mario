using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S; // Phím để vào ống
    public Vector3 enterDirection = Vector3.down; // Hướng vào ống
    public Vector3 exitDirection = Vector3.zero; // Hướng ra khỏi ống

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(enterKeyCode))
            {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        playerMovement.enabled = false; // Vô hiệu hóa điều khiển người chơi
        playerRigidbody.bodyType = RigidbodyType2D.Kinematic; // Tắt vật lý để không bị trọng lực kéo
        playerRigidbody.linearVelocity = Vector2.zero; // Đặt vận tốc về 0

        // Lấy bounds của collider để tính vị trí chính xác
        Collider2D pipeCollider = GetComponent<Collider2D>();
        Vector3 enteredPosition = pipeCollider.bounds.center + enterDirection * pipeCollider.bounds.extents.magnitude;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f); // Thời gian chờ trong ống

        bool underground = connection.localPosition.y < 0f;
        Camera.main.GetComponent<CameraMovement>().SetUnderground(underground);

        if(connection != null && exitDirection != Vector3.zero)
        {
            Collider2D connectionCollider = connection.GetComponent<Collider2D>();
            player.position = connectionCollider.bounds.center - exitDirection * connectionCollider.bounds.extents.magnitude;
            yield return Move(player, connectionCollider.bounds.center + exitDirection * connectionCollider.bounds.extents.magnitude, Vector3.one);
        }
        else if(connection != null)
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        playerMovement.enabled = true; // Kích hoạt lại điều khiển người chơi
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic; // Bật lại vật lý
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
