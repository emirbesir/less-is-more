using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeath player))
        {
            PlayerDeath.SetRespawnPoint(transform.position);
            Debug.Log($"Checkpoint set to: {transform.position}");
        }
    }
}
