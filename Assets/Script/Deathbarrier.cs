using UnityEngine;

public class Deathbarrier : MonoBehaviour
{
    // Hàm xử lý chạm vào biên thì nhân vật sẽ mất 1 mạng
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            player.Death();
        }
        else
        {
            Destroy(other.gameObject); 
        }
    }
}
