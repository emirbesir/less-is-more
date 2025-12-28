using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    // TODO: Go to http://dreamlo.com, click "Get Codes", and paste them here:
    private const string privateCode = "6LZifCjpLUyqx1_JSfhRlQhQurNXNwuUuA9HogRvkMIg"; 
    private const string publicCode = "695093db8f40bbcf806b07cf"; 
    private const string webURL = "http://dreamlo.com/lb/";

    [Header("Game Jam Settings")]
    [Tooltip("If checked, this script will automatically grab the score from GameManager when the scene starts.")]
    public bool autoUploadOnStart = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // If this script is only in the End Scene, we can just grab the score immediately here.
        if (autoUploadOnStart && GameManager.Instance != null)
        {
            // Create a temporary ID/Name for the leaderboard
            string playerName = "Player_" + UnityEngine.Random.Range(1000, 9999);
            
            Debug.Log($"Auto-uploading score: {GameManager.Instance.TotalDeathsInGame}");
            UploadDeaths(playerName, GameManager.Instance.TotalDeathsInGame);
        }
    }

    // 1. Upload the death count
    public void UploadDeaths(string username, int totalDeaths)
    {
        // For death counters, we usually want the "Low" score to be best.
        // Dreamlo sorts by "High" score by default. 
        // A jam hack: Send deaths as a negative number? 
        // OR: Just upload normally and we handle the math logic ourselves (Lower is better).
        // Let's upload normally.
        StartCoroutine(UploadScoreRoutine(username, totalDeaths));
    }

    IEnumerator UploadScoreRoutine(string username, int score)
    {
        // Dreamlo URL format: /add/Username/Score
        string requestUrl = webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score;
        
        using (UnityWebRequest www = UnityWebRequest.Get(requestUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error uploading score: " + www.error);
            }
            else
            {
                Debug.Log("Score uploaded!");
            }
        }
    }

    // 2. Get scores and calculate percentile
    // Callback returns a float (0 to 100 representing how many people you beat)
    public void GetPercentile(int myDeathCount, Action<float> onPercentileCalculated)
    {
        StartCoroutine(GetScoresAndCalculateRoutine(myDeathCount, onPercentileCalculated));
    }

    IEnumerator GetScoresAndCalculateRoutine(int myDeathCount, Action<float> callback)
    {
        // "/pipe" gives us a clean format to parse: Name|Score|Seconds
        string requestUrl = webURL + publicCode + "/pipe";

        using (UnityWebRequest www = UnityWebRequest.Get(requestUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading: " + www.error);
                callback?.Invoke(0f); // Default to 0 on error
            }
            else
            {
                string textData = www.downloadHandler.text;
                float percentile = CalculateBetterThanPercentage(textData, myDeathCount);
                callback?.Invoke(percentile);
            }
        }
    }

    private float CalculateBetterThanPercentage(string csvData, int myDeaths)
    {
        string[] rows = csvData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        if (rows.Length == 0) return 100f; // You are the first player!

        int totalPlayers = rows.Length;
        int playersWorseThanMe = 0; // "Worse" means they have MORE deaths than me

        foreach (string row in rows)
        {
            string[] entries = row.Split('|');
            if (entries.Length >= 2)
            {
                if (int.TryParse(entries[1], out int otherPlayerDeaths))
                {
                    // Since fewer deaths is better:
                    // If they have MORE deaths than me, I am better than them.
                    if (otherPlayerDeaths > myDeaths)
                    {
                        playersWorseThanMe++;
                    }
                }
            }
        }

        // Calculate percentage
        // If I have 50 deaths, and friend has 120. Friend is "Worse".
        // If 10 people played, and 5 have > 50 deaths, I am better than 50%.
        float result = ((float)playersWorseThanMe / (float)totalPlayers) * 100f;
        return result;
    }
}