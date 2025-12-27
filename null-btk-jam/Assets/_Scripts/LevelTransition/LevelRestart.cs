using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.J;

    void Update()
    {
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

