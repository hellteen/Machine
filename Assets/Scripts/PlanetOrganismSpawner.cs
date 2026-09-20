using UnityEngine;

public class PlanetOrganismSpawner : MonoBehaviour
{
    [SerializeField] private GameObject jupiterOrganism;
    [SerializeField] private GameObject marsOrganism;
    [SerializeField] private GameObject neptuneOrganism;
    [SerializeField] private GameObject uranusOrganism;

    private void Start()
    {
        SpawnOrganism();
    }

    private void SpawnOrganism()
    {
        string planetName = PlanetSelectionManager.SelectedPlanetJson.name;

        if (planetName == "jupiter")
        {
            jupiterOrganism.SetActive(true);
        }
        else if (planetName == "mars")
        {
            marsOrganism.SetActive(true);
        }
        else if (planetName == "neptune")
        {
            neptuneOrganism.SetActive(true);
        }
        else if (planetName == "uranus")
        {
            uranusOrganism.SetActive(true);
        }
    }
}