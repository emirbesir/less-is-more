using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject checkpointTextPopup;
    [SerializeField] private Light2D light2D;

    private bool isActivated = false;
    private AudioSource sfxSource;

    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
        isActivated = false;
    }

    private void Start()
    {
        light2D.intensity = 0.5f;
        light2D.pointLightOuterRadius = 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeath player))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        if (isActivated) return;
        
        isActivated = true;
        PlayerDeath.SetRespawnPoint(transform.position);
        light2D.intensity = 2f;
        light2D.pointLightOuterRadius = 3f;
        sfxSource.Play();
        Instantiate(checkpointTextPopup, transform.position + Vector3.up, Quaternion.identity);
        Debug.Log($"Checkpoint set to: {transform.position}");
    }
}