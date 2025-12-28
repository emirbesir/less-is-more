using UnityEngine;
using System.Collections.Generic;

public class Spike : MonoBehaviour
{
    [SerializeField] private List<GameObject> spikeTextPopup;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerDeath player))
        {
            player.Die();
            PickRandomPopup();
        }
    }

    // Also handle Trigger if spikes are triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeath player))
        {
            player.Die();
            PickRandomPopup();
        }
    }
    
    private void PickRandomPopup()
    {
        if (spikeTextPopup.Count == 0) return;
        int randomIndex = Random.Range(0, spikeTextPopup.Count);
        Instantiate(spikeTextPopup[randomIndex], transform.position + Vector3.up, Quaternion.identity);
    }
}
