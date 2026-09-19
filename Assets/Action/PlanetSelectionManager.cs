using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlanetSelectionManager : MonoBehaviour
{
   
    public static TextAsset SelectedPlanetJson;

    [Header("Настройки перехода")]
    [Tooltip("Название сцены симуляции")]
    [SerializeField] private string targetSceneName = "Simulation";

    [Header("Визуал затемнения")]
    [Tooltip("Полноэкранный Image (перетащи сюда FadeScreen из Canvas)")]
    [SerializeField] private Image fadeScreen;
    [Tooltip("Длительность затемнения в секундах")]
    [SerializeField] private float fadeDuration = 0.8f;
    [Tooltip("Цвет затемнения")]
    [SerializeField] private Color fadeColor = Color.black;

   
    private PlanetSelection selectedPlanet;
    private bool isStarting = false;

    private void Awake()
    {
        
        if (fadeScreen != null)
        {
            fadeScreen.raycastTarget = false;
            Color c = fadeColor;
            c.a = 0f;
            fadeScreen.color = c;
            fadeScreen.gameObject.SetActive(false);
        }
    }

    // -----------------------------------------
    // ВЫБОР ПЛАНЕТЫ
    // -----------------------------------------

    public void SelectPlanet(PlanetSelection planet)
    {
        if (planet == null) return;

        selectedPlanet = planet;
        Debug.Log("Selected planet: " + planet.gameObject.name);
    }

    // -----------------------------------------
    // ЗАПУСК СИМУЛЯЦИИ
    // -----------------------------------------

    public void StartSimulation()
    {
        Debug.Log("=== START SIMULATION CLICKED ===");

        
        if (isStarting) return;

        if (selectedPlanet == null)
        {
            Debug.LogError("ERROR: selectedPlanet == null");
            return;
        }

        Debug.Log("Selected planet object: " + selectedPlanet.gameObject.name);

        if (selectedPlanet.PlanetJson == null)
        {
            Debug.LogError("ERROR: PlanetJson == null for " + selectedPlanet.gameObject.name);
            return;
        }

        Debug.Log("JSON found: " + selectedPlanet.PlanetJson.name);

        
        SelectedPlanetJson = selectedPlanet.PlanetJson;
        Debug.Log("STATIC JSON SAVED: " + SelectedPlanetJson.name);

        
        StartCoroutine(FadeAndLoadRoutine());
    }

    private IEnumerator FadeAndLoadRoutine()
    {
        isStarting = true;

        
        if (fadeScreen == null)
        {
            Debug.LogWarning("[PlanetSelectionManager] Поле Fade Screen пустое! Загружаем без затемнения.");
            SceneManager.LoadScene(targetSceneName);
            yield break;
        }

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

        
        yield return null;

        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}