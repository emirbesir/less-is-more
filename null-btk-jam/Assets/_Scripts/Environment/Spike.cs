using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerDeath player))
        {
            player.Die();
        }
    }

    // Also handle Trigger if spikes are triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeath player))
        {
            player.Die();
        }
    }
}
