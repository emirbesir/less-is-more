using TMPro;
using UnityEngine;

public class TotalDeathCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text totalDeathCountText;
    
    private void Start()
    {
        totalDeathCountText.text = $"Toplam Ceset Sayısı: {GameManager.Instance.TotalDeathsInGame}";
    }
}
