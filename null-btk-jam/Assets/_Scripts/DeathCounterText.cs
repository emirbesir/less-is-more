using System;
using TMPro;
using UnityEngine;

public class DeathCounterText : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerDeathText;

    private void Start()
    {
        UpdateDeathText(GameManager.Instance.TotalDeathsInLevel, GameManager.Instance.TotalDeathsAllowedInCurrentLevel);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnDeathCountChanged += UpdateDeathText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnDeathCountChanged -= UpdateDeathText;
    }
    
    private void UpdateDeathText(int currentDeaths, int maxDeaths)
    {
        _playerDeathText.text = $"Ceset Sayısı: {currentDeaths} / {maxDeaths}";
    }
}
