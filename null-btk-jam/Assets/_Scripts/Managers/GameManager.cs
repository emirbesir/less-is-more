using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<int, int> OnDeathCountChanged;
    
    [SerializeField] private DifficultySO difficulty;
    [SerializeField] private PlayerDeath playerDeath;
    
    public static GameManager Instance { get; private set; }

    public int RoomsCleared { get; private set; } = 0;
    public int TotalDeathsInLevel { get; private set; } = 0;
    public int TotalDeathsAllowedInCurrentLevel => 
        difficulty.maxDeathsAllowedInLevel[LevelManager.Instance.LevelIndex];
    public int TotalDeathsInGame { get; private set; } = 0;

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
    
    private void HandlePlayerDeath()
    {
        TotalDeathsInLevel++;
        TotalDeathsInGame++;
        Debug.Log($"Player Died! Total Deaths in Level: {TotalDeathsInLevel}, Total Deaths in Game: {TotalDeathsInGame}");
        playerDeath.OnDeath -= HandlePlayerDeath;
        
        OnDeathCountChanged?.Invoke(TotalDeathsInLevel, difficulty.maxDeathsAllowedInLevel[LevelManager.Instance.LevelIndex]);
        if (difficulty.maxDeathsAllowedInLevel[LevelManager.Instance.LevelIndex] <= TotalDeathsInLevel)
        {
            Debug.Log("Max deaths exceeded for this level. Resetting run.");
            ResetRun();
        }
    }
    
    public void CompleteRoom()
    {
        RoomsCleared++;
        TotalDeathsInLevel = 0;
        Debug.Log($"Room Complete! Total: {RoomsCleared}");
    }

    public void ResetRun()
    {
        RoomsCleared = 0;
        TotalDeathsInLevel = 0;
        TotalDeathsInGame = 0;
        SceneManager.LoadScene("StartMenu"); // Make sure you have a scene named this, or change it
    }

    public void ResetLevelDeathCount()
    {
        TotalDeathsInLevel = 0;
    }
    
    public void SetPlayerDeathReference(PlayerDeath deathComponent)
    {
        playerDeath = deathComponent;
        playerDeath.OnDeath += HandlePlayerDeath;
    }
}