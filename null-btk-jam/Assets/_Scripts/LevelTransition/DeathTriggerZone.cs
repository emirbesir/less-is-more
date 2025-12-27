using UnityEngine;

public class DeathTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.RestartLevel();
            }
        }
    }
}
