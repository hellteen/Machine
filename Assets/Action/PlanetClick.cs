
using System.Diagnostics;
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
    }


    private void OnMouseDown()
    {
        if (manager == null)
        {
            Debug.LogError(
                "PlanetSelectionManager not found!"
            );

            return;
        }

        manager.SelectPlanet(planet);
    }
}
```
