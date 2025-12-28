using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButtons : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayButtonClickSound();
    }
    
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
        PlayButtonClickSound();
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayButtonClickSound();
    }
    
    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
