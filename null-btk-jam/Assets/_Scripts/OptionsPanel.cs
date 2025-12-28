using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (mixer != null && volumeSlider != null)
        {
            float volume;
            mixer.GetFloat("MasterVolume", out volume);
            volumeSlider.value = Mathf.Pow(10, volume / 20); // Convert dB to linear
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void SetVolume(float volume)
    {
        if (mixer != null)
        {
            float dB;
            if (volume > 0)
                dB = 20f * Mathf.Log10(volume); // Convert linear to dB
            else
                dB = -80f; // Minimum volume in dB

            mixer.SetFloat("MasterVolume", dB);
        }
    }
    
}
