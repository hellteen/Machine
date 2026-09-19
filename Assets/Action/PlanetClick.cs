
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlanetClick : MonoBehaviour
{
    private PlanetSelection planet;
    private PlanetSelectionManager manager;


    private void Start()
    {
        planet =
            GetComponent<PlanetSelection>();

        manager =
            FindFirstObjectByType<PlanetSelectionManager>();


        if (planet == null)
        {
            Debug.LogError(
                "PlanetSelection component not found on " +
                gameObject.name
            );
        }

        if (manager == null)
        {
            Debug.LogError(
                "PlanetSelectionManager not found!"
            );
        }
    }


    private void OnMouseDown()
    {
        Debug.Log(
            "CLICKED PLANET: " +
            gameObject.name
        );


        if (manager == null)
        {
            Debug.LogError(
                "PlanetSelectionManager not found!"
            );

            return;
        }


        if (planet == null)
        {
            Debug.LogError(
                "PlanetSelection not found!"
            );

            return;
        }


        manager.SelectPlanet(planet);
    }
}
