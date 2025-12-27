using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Transition UI")]
    [SerializeField] private CanvasGroup transitionCanvasGroup;
    [SerializeField] private RectTransform transitionImage;
    [SerializeField] private float transitionDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip transitionSound;

    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Make sure canvas is a child so it persists with DontDestroyOnLoad
        if (transitionCanvasGroup != null && transitionCanvasGroup.transform.parent != transform)
        {
            transitionCanvasGroup.transform.SetParent(transform);
        }

        // Start with transition hidden
        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        if (transitionImage != null)
        {
            transitionImage.localScale = Vector3.zero;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlaySceneTransition(sceneIndex));
        }
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No more levels to load. Returning to first level.");
            LoadScene(0);
        }
    }

    public void RestartLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentIndex);
    }

    private IEnumerator PlaySceneTransition(int sceneIndex)
    {
        isTransitioning = true;

        // Show canvas
        transitionCanvasGroup.alpha = 1;
        transitionCanvasGroup.blocksRaycasts = true;
        transitionImage.localScale = Vector3.zero;

        // Scale IN (cover screen)
        yield return StartCoroutine(ScaleTransition(Vector3.zero, Vector3.one * 1.15f, transitionDuration, EaseInQuad));

        // Play sound
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        // Load scene
        yield return SceneManager.LoadSceneAsync(sceneIndex);

        // Small pause for polish
        yield return new WaitForSeconds(0.1f);

        // Scale OUT (reveal new scene)
        transitionImage.localScale = Vector3.one * 1.15f;
        yield return StartCoroutine(ScaleTransition(Vector3.one * 1.15f, Vector3.zero, transitionDuration, EaseOutQuad));

        // Hide canvas
        transitionCanvasGroup.alpha = 0;
        transitionCanvasGroup.blocksRaycasts = false;

        isTransitioning = false;
    }

    private IEnumerator ScaleTransition(Vector3 from, Vector3 to, float duration, System.Func<float, float> easeFunc)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = easeFunc(t);

            transitionImage.localScale = Vector3.Lerp(from, to, easedT);
            yield return null;
        }

        transitionImage.localScale = to;
    }

    // Easing functions (like DOTween)
    private float EaseInQuad(float t)
    {
        return t * t;
    }

    private float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }
}

