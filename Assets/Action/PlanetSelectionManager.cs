```csharp
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlanetSelectionManager : MonoBehaviour
{
    // JSON выбранной планеты.
    // Он сохранится при переходе на другую сцену.
    public static TextAsset SelectedPlanetJson;

    [Header("Название сцены симуляции")]
    [SerializeField]
    private string targetSceneName = "Simulation";

    // Текущая выбранная планета.
    private PlanetSelection selectedPlanet;


    // -----------------------------------------
    // ВЫБОР ПЛАНЕТЫ
    // -----------------------------------------

    public void SelectPlanet(PlanetSelection planet)
    {
        if (planet == null)
        {
            return;
        }

        selectedPlanet = planet;

        Debug.Log(
            "Selected planet: " +
            planet.gameObject.name
        );
    }


    // -----------------------------------------
    // ЗАПУСК СИМУЛЯЦИИ
    // -----------------------------------------

    public void StartSimulation()
    {
        if (selectedPlanet == null)
        {
            Debug.LogWarning(
                "Planet is not selected!"
            );

            return;
        }

        if (selectedPlanet.PlanetJson == null)
        {
            Debug.LogError(
                "Selected planet does not have a JSON file!"
            );

            return;
        }

        // Запоминаем JSON выбранной планеты.
        SelectedPlanetJson =
            selectedPlanet.PlanetJson;

        Debug.Log(
            "Loading planet: " +
            selectedPlanet.PlanetJson.name
        );

        // Переходим в сцену симуляции.
        SceneManager.LoadScene(
            targetSceneName
        );
    }
}
```
