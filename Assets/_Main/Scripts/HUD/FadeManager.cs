using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage != null)
        {
            // Aseguramos que el panel esté visible
            fadeImage.color = new Color(0, 0, 0, 1);
            fadeImage.raycastTarget = true;
            StartCoroutine(FadeIn());
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(FadeOut(nombreEscena));
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime / fadeDuration;
            color.a = Mathf.Clamp01(t);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.raycastTarget = false;
    }

    IEnumerator FadeOut(string escenaDestino)
    {
        Color color = fadeImage.color;
        float t = 0f;

        fadeImage.raycastTarget = true;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            color.a = Mathf.Clamp01(t);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(escenaDestino);
    }
}
