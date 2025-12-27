using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int RoomsCleared { get; private set; } = 0;

    private void Awake()
    {
        // Singleton pattern: Ensure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object alive when switching scenes
    }

    public void CompleteRoom()
    {
        RoomsCleared++;
        Debug.Log($"Room Complete! Total: {RoomsCleared}");
    }

    public void ResetRun()
    {
        RoomsCleared = 0;
        SceneManager.LoadScene("StartMenu"); // Make sure you have a scene named this, or change it
    }
}