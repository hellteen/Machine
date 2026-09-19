using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private float tickInterval = 1f;

    public PlanetData Planet
    {
        get;
        private set;
    }

    public DecisionResult LastDecision
    {
        get;
        private set;
    }

    private DecisionEngine decisionEngine;

    private ActionSystem actionSystem;

    private EvolutionSystem evolutionSystem;

    private float tickTimer;

    private void Start()
    {
        decisionEngine =
            new DecisionEngine();

        actionSystem =
            new ActionSystem();

        evolutionSystem =
            new EvolutionSystem();

        LoadPlanet();

        if (Planet == null)
        {
            Debug.LogError(
                "Simulation stopped: planet was not loaded."
            );

            return;
        }

        InitializePopulation();

        Debug.Log(
            "Initial population: " +
            Planet.organisms.Count
        );
    }

    private void Update()
    {
        tickTimer +=
            Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;

            SimulationTick();
        }
    }

    private void LoadPlanet()
    {
        TextAsset planetJson =
            PlanetSelectionManager.SelectedPlanetJson;

        if (planetJson == null)
        {
            Debug.LogError(
                "No planet JSON was selected!"
            );

            return;
        }

        Planet =
            JsonUtility.FromJson<PlanetData>(
                planetJson.text
            );

        if (Planet == null)
        {
            Debug.LogError(
                "Failed to load planet JSON!"
            );

            return;
        }

        Debug.Log(
            "Loaded planet: " +
            Planet.name
        );
    }

    private void InitializePopulation()
    {
        if (Planet.organisms == null)
        {
            Planet.organisms =
                new List<OrganismData>();
        }

        foreach (OrganismData organism in Planet.organisms)
        {
            if (organism.genome == null)
            {
                organism.genome =
                    evolutionSystem.CreateRandomGenome();
            }

            if (organism.direction == null)
            {
                Vector2 randomDirection =
                    UnityEngine.Random.insideUnitCircle.normalized;

                organism.direction =
                    new DirectionData
                    {
                        x = randomDirection.x,
                        y = randomDirection.y
                    };
            }
        }
    }

    private void SimulationTick()
    {
        if (
            Planet == null ||
            Planet.organisms == null
        )
        {
            return;
        }

        List<OrganismData> newborns =
            new List<OrganismData>();

        List<OrganismData> population =
            new List<OrganismData>(
                Planet.organisms
            );

        foreach (
            OrganismData organism
            in population)
        {
            if (
                organism.state.health <= 0)
            {
                continue;
            }

            LastDecision =
                decisionEngine.MakeDecision(
                    organism,
                    Planet.environment
                );

            bool reproduced =
                actionSystem.Execute(
                    organism,
                    Planet.environment,
                    LastDecision
                );

            if (reproduced)
            {
                OrganismData child =
                    evolutionSystem
                        .CreateOffspring(
                            organism
                        );

                newborns.Add(child);

                Debug.Log(
                    "Organism " +
                    organism.id +
                    " reproduced. " +
                    "Child: " +
                    child.id
                );
            }
        }

        Planet.organisms.AddRange(
            newborns
        );

        RemoveDeadOrganisms();

        UpdateEnvironment();

        Debug.Log(
            "Population: " +
            Planet.organisms.Count
        );
    }

    private void RemoveDeadOrganisms()
    {
        Planet.organisms.RemoveAll(
            organism =>
                organism.state.health <= 0
        );
    }

    private void UpdateEnvironment()
    {
        Planet.environment.resources +=
            Planet.environment
                .resourceRegeneration;

        Planet.environment.resources =
            Mathf.Clamp(
                Planet.environment.resources,
                0f,
                2000f
            );
    }
}