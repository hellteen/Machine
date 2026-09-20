    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;
    using Random = UnityEngine.Random;
    using Debug = UnityEngine.Debug;

    public class PopulationRenderer : MonoBehaviour
    {
        [SerializeField]
        private SimulationManager simulation;

    [SerializeField]
    private GameObject jupiterOrganismPrefab;

    [SerializeField]
    private GameObject marsOrganismPrefab;

    [SerializeField]
    private GameObject uranusOrganismPrefab;

    [SerializeField]
    private GameObject neptuneOrganismPrefab;

    [SerializeField]
        private Transform organismParent;

        private Dictionary<int, OrganismView> views =
            new Dictionary<int, OrganismView>();

        private void Update()
        {
            if (
                simulation == null ||
                simulation.Planet == null ||
                simulation.Planet.organisms == null
            )
            {
                return;
            }

            UpdatePopulation();
        }

        private void UpdatePopulation()
        {
            List<OrganismData> organisms =
                simulation.Planet.organisms;

            foreach (
                OrganismData organism
                in organisms)
            {
                if (!views.ContainsKey(organism.id))
                {
                    CreateOrganismView(
                        organism
                    );
                }
            }

            List<int> idsToRemove =
                new List<int>();

            foreach (
                var pair
                in views)
            {
                bool organismExists =
                    false;

                foreach (
                    OrganismData organism
                    in organisms)
                {
                    if (
                        organism.id ==
                        pair.Key)
                    {
                        organismExists = true;
                        break;
                    }
                }

                if (!organismExists)
                {
                    Destroy(
                        pair.Value.gameObject
                    );

                    idsToRemove.Add(
                        pair.Key
                    );
                }
            }

            foreach (
                int id
                in idsToRemove)
            {
                views.Remove(id);
            }
        }
    private GameObject GetOrganismPrefab()
    {
        switch (simulation.Planet.name)
        {
            case "Jupiter":
                return jupiterOrganismPrefab;

            case "Mars":
                return marsOrganismPrefab;

            case "Earth":
                return uranusOrganismPrefab;

            case "Venus":
                return neptuneOrganismPrefab;

            default:
                return null;
        }
    }
    private void CreateOrganismView(
            OrganismData organism)
        {
        GameObject prefab = GetOrganismPrefab();

        if (prefab == null)
        {
            Debug.LogError(
                "No organism prefab found for planet: " +
                simulation.Planet.name
            );

            return;
        }

        GameObject obj =
            Instantiate(
                prefab,
                organismParent
            );

        OrganismView view =
                obj.GetComponent<OrganismView>();

            if (view == null)
            {
                Debug.LogError(
                    "Organism prefab " +
                    "does not have OrganismView!"
                );

                Destroy(obj);

                return;
            }

            view.Initialize(
                organism
            );

            views.Add(
                organism.id,
                view
            );
        }
    }