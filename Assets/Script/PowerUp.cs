using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                // Add coin to player's inventory
                GameManager.Instance.AddCoin();
                break;
            case Type.ExtraLife:
                // Increase player's life count
                GameManager.Instance.AddLife();
                break;
            case Type.MagicMushroom:
                // Trigger growth effect on player
                player.GetComponent<PlayerStatus>().Grow();
                break;
            case Type.Starpower:
                // Grant temporary invincibility to player
                player.GetComponent<PlayerStatus>().Starpower();
                break;
        }

        Destroy(gameObject);
    }
}
