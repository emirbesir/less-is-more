using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool isExiting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isExiting)
        {
            isExiting = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRoom();
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadNextLevel();
            }
        }
    }
}