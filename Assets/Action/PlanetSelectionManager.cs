
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
        Debug.Log("=== START SIMULATION CLICKED ===");

        if (selectedPlanet == null)
        {
            Debug.LogError(
                "ERROR: selectedPlanet == null"
            );

            return;
        }

        Debug.Log(
            "Selected planet object: " +
            selectedPlanet.gameObject.name
        );


        if (selectedPlanet.PlanetJson == null)
        {
            Debug.LogError(
                "ERROR: PlanetJson == null for " +
                selectedPlanet.gameObject.name
            );

            return;
        }


        Debug.Log(
            "JSON found: " +
            selectedPlanet.PlanetJson.name
        );


        SelectedPlanetJson =
            selectedPlanet.PlanetJson;


        Debug.Log(
            "STATIC JSON SAVED: " +
            SelectedPlanetJson.name
        );


        SceneManager.LoadScene(
            targetSceneName
        );
    }


}

