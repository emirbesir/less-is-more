using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.T;

    void Update()
    {
        // Check for restart input - keyboard only since MobileButton calls RestartLevel directly
        if (Input.GetKeyDown(restartKey))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RestartLevel();
        }
    }
}

