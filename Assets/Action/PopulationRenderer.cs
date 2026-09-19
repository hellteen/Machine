using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;


public class PopulationRenderer : MonoBehaviour
{
    // Ссылка на SimulationManager
    // из существующей сцены.
    [SerializeField]
    private SimulationManager simulation;


    // Наш prefab организма.
    [SerializeField]
    private GameObject organismPrefab;


    // Родительский объект,
    // внутри которого будут находиться
    // созданные организмы.
    [SerializeField]
    private Transform organismParent;


    // Связь:
    //
    // ID организма
    //       ↓
    // его GameObject
    //
    // Благодаря этому мы понимаем,
    // какой GameObject соответствует
    // какому организму.

    private Dictionary<int, OrganismView> views =
        new Dictionary<int, OrganismView>();


    // -----------------------------------------
    // UPDATE
    // -----------------------------------------

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


    // -----------------------------------------
    // СИНХРОНИЗАЦИЯ
    // -----------------------------------------

    private void UpdatePopulation()
    {
        List<OrganismData> organisms =
            simulation.Planet.organisms;


        // -------------------------------------
        // СОЗДАЁМ НОВЫХ
        // -------------------------------------

        foreach (
            OrganismData organism
            in organisms)
        {
            // Если для этого организма
            // ещё нет GameObject,
            // создаём его.

            if (!views.ContainsKey(organism.id))
            {
                CreateOrganismView(
                    organism
                );
            }
        }


        // -------------------------------------
        // УДАЛЯЕМ УМЕРШИХ
        // -------------------------------------

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


            // Если организма больше нет
            // в симуляции — удаляем его
            // GameObject.

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


        // Удаляем записи из Dictionary.

        foreach (
            int id
            in idsToRemove)
        {
            views.Remove(id);
        }
    }


    // -----------------------------------------
    // СОЗДАНИЕ GAMEOBJECT
    // -----------------------------------------

    private void CreateOrganismView(
        OrganismData organism)
    {
        GameObject obj =
            Instantiate(
                organismPrefab,
                organismParent
            );


        // Получаем OrganismView
        // с созданного объекта.

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


        // Передаём GameObject'у
        // данные настоящего организма.

        view.Initialize(
            organism
        );


        // Запоминаем связь.
        views.Add(
            organism.id,
            view
        );
    }
}