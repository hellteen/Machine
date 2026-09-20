using UnityEngine;

public class PlanetClick : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Поставьте галочку у Юпитера, чтобы он был выбран автоматически на старте")]
    [SerializeField] private bool isDefaultSelected = false;

    private PlanetSelection planet;
    private PlanetSelectionManager manager;

    private void Awake()
    {
        planet = GetComponent<PlanetSelection>();
    }

    private void Start()
    {
        FindManager();

        
        if (isDefaultSelected)
        {
            SelectThisPlanet();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("CLICKED PLANET: " + gameObject.name);
        SelectThisPlanet();
    }

    public void SelectThisPlanet()
    {
        
        if (manager == null)
        {
            FindManager();
        }

        if (manager == null)
        {
            Debug.LogWarning($"[PlanetClick] Не найден PlanetSelectionManager на сцене для {gameObject.name}");
            return;
        }

        if (planet == null)
        {
            planet = GetComponent<PlanetSelection>();
            if (planet == null)
            {
                Debug.LogError($"[PlanetClick] На объекте {gameObject.name} отсутствует компонент PlanetSelection!");
                return;
            }
        }

        manager.SelectPlanet(planet);
    }

    private void FindManager()
    {
        manager = FindFirstObjectByType<PlanetSelectionManager>();
    }
}