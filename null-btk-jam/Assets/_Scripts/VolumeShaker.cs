using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening; // You already have DOTween installed!

public class VolumeShaker : MonoBehaviour
{
    public static VolumeShaker Instance;

    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Bloom bloom;

    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        
        // Get overrides from the volume profile
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out bloom);
    }

    public void ShakeDeath()
    {
        // 1. Spikes Chromatic Aberration (Glitch effect)
        if(chromaticAberration != null)
        {
            chromaticAberration.intensity.value = 1f;
            DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0f, 0.5f);
        }

        // 2. Distort Lens (Impact)
        if(lensDistortion != null)
        {
            lensDistortion.intensity.value = -0.5f;
            DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0f, 0.3f).SetEase(Ease.OutBack);
        }
        
        // 3. Bloom Flash
        if(bloom != null)
        {
            float originalIntensity = 2.5f; // Set this to your default
            bloom.intensity.value = 15f; // Blind the player momentarily
            DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, originalIntensity, 0.5f);
        }
    }
}