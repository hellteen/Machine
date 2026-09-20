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
        private GameObject organismPrefab;

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

        private void CreateOrganismView(
            OrganismData organism)
        {
            GameObject obj =
                Instantiate(
                    organismPrefab,
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