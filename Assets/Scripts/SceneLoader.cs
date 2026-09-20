using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Настройки сцены")]
    [Tooltip("Название сцены, на которую нужно перейти")]
    [SerializeField] private string targetSceneName = "Simulation";

    [Header("Визуал затемнения")]
    [Tooltip("Полноэкранный Image затемнения")]
    [SerializeField] private Image fadeScreen;
    [Tooltip("Время затемнения в секундах")]
    [SerializeField] private float fadeDuration = 0.8f;
    [Tooltip("Цвет затемнения")]
    [SerializeField] private Color fadeColor = Color.black;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (fadeScreen != null)
        {
            fadeScreen.gameObject.SetActive(false);
        }
    }

    public void LoadSimulationScene()
    {
        Debug.Log("<color=cyan>[SceneLoader]</color> 1. Клик зарегистрирован, запуск перехода!");

        if (isTransitioning)
        {
            Debug.LogWarning("[SceneLoader] Переход уже идёт, повторный клик проигнорирован.");
            return;
        }

        StartCoroutine(FadeAndLoadRoutine());
    }

    private IEnumerator FadeAndLoadRoutine()
    {
        isTransitioning = true;

        
        if (fadeScreen == null)
        {
            Debug.LogError("<color=red>[SceneLoader КРИТИЧЕСКАЯ ОШИБКА]</color> Поле 'Fade Screen' в инспекторе ПУСТОЕ (None)! Скрипт не знает, что затемнять, и грузит сцену мгновенно.");
            SceneManager.LoadScene(targetSceneName);
            yield break;
        }

        Debug.Log("<color=green>[SceneLoader]</color> 2. Затемнение началось, ждём " + fadeDuration + " сек...");

        if (fadeDuration <= 0.1f) fadeDuration = 0.8f;

        fadeScreen.gameObject.SetActive(true);
        fadeScreen.raycastTarget = true;

        Color c = fadeColor;
        c.a = 0f;
        fadeScreen.color = c;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            c.a = Mathf.SmoothStep(0f, 1f, t);
            fadeScreen.color = c;

            yield return null;
        }

        c.a = 1f;
        fadeScreen.color = c;

        Debug.Log("<color=yellow>[SceneLoader]</color> 3. Экран полностью чёрный. Загружаем: " + targetSceneName);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}