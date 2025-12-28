using UnityEngine;
using TMPro;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        if (resultText != null) 
            resultText.text = "Sıralama Hesaplanıyor...";
        
        // Slight delay to ensure the upload (from GameManager) has started processing
        StartCoroutine(FetchRankingRoutine());
    }

    IEnumerator FetchRankingRoutine()
    {
        // Wait 1 second to let the upload finish on the server side
        yield return new WaitForSeconds(1.0f);

        int myDeaths = GameManager.Instance.TotalDeathsInGame;

        LeaderboardManager.Instance.GetPercentile(myDeaths, (percentile) => 
        {
            // Format to 1 decimal place, e.g., "50.5%"
            string formattedPercent = percentile.ToString("F1");
            
            if (percentile >= 50)
            {
                resultText.text = $"Toplam Ölüm: {myDeaths}\n\nHarika! oynayanların <color=green>%{formattedPercent}</color>'ından daha iyisin!";
            }
            else
            {
                resultText.text = $"Toplam Ölüm: {myDeaths}\n\nOynayanların <color=yellow>%{formattedPercent}</color>'ından daha iyisin. \nDaha iyi olabilirsin!";
            }
        });
    }
}