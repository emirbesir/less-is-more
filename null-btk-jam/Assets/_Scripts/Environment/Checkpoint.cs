using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Light2D _light2D;
    private void Start()
    {
        _light2D.intensity = 0.5f;
        _light2D.pointLightOuterRadius = 1.5f;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeath player))
        {
            PlayerDeath.SetRespawnPoint(transform.position);
            _light2D.intensity = 2f;
            _light2D.pointLightOuterRadius = 3f;
            Debug.Log($"Checkpoint set to: {transform.position}");
        }
    }
}
