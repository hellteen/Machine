using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using Debug = UnityEngine.Debug;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private float tickInterval = 0.01f;

    [SerializeField]
    private PlanetParameters planetParameters;

    [SerializeField]
    private PopulationStatus populationStatus;

    [SerializeField]
    private TextAsset planetJson;

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

    private int previousPopulation;
    private int tickCount;

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

        if (planetParameters != null)
        {
            planetParameters.Initialize(Planet);
        }

        if (populationStatus != null)
        {
            populationStatus.Initialize(Planet);
        }

    
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

        foreach (
            OrganismData organism
            in Planet.organisms)
        {
            if (organism.genome == null)
            {
                organism.genome =
                    evolutionSystem.CreateRandomGenome();
            }

            if (organism.direction == null)
            {
                Vector2 randomDirection =
                    UnityEngine.Random
                        .insideUnitCircle
                        .normalized;

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

        tickCount++;

        int populationBefore =
            Planet.organisms.Count;

        List<OrganismData> newborns =
            new List<OrganismData>();

        List<OrganismData> population =
            new List<OrganismData>(
                Planet.organisms
            );

        int reproducedCount = 0;

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

                reproducedCount++;
            }
        }

  
        Planet.organisms.AddRange(
            newborns
        );


        int populationBeforeDeath =
            Planet.organisms.Count;

        RemoveDeadOrganisms();

        int deadCount =
            populationBeforeDeath -
            Planet.organisms.Count;

     
        UpdateEnvironment();

        if (planetParameters != null)
        {
            planetParameters.UpdateSliders();
        }

        if (populationStatus != null)
        {
            populationStatus.UpdateStats();
        }

        int currentPopulation =
            Planet.organisms.Count;

  
        if (reproducedCount > 0)
        {
            SimulationEvents.Add(
                "🧬 Родилось организмов: " +
                reproducedCount
            );
        }

       
        if (deadCount > 0)
        {
            SimulationEvents.Add(
                "☠️ Погибло организмов: " +
                deadCount
            );
        }

       
        if (
            currentPopulation != previousPopulation ||
            tickCount % 20 == 0
        )
        {
            SimulationEvents.Add(
                "👥 Популяция: " +
                currentPopulation
            );

            previousPopulation =
                currentPopulation;
        }
        if (currentPopulation == 0)
        {
            SimulationEvents.Add(
                "Популяция исчезла"
            );
            return;
        }

        Debug.Log(
            "Популяция: " +
            currentPopulation
        );
    }

    private void RemoveDeadOrganisms()
    {
        int populationBefore =
            Planet.organisms.Count;

        Planet.organisms.RemoveAll(
            organism =>
                organism.state.health <= 0
        );

        int deadCount =
            populationBefore -
            Planet.organisms.Count;

        if (deadCount > 0)
        {
            SimulationEvents.Add(
                "Умерло: " +
                deadCount +
                " организма(ов)"
            );
        }
    }

    private void CheckPopulationChanges()
    {
        int currentPopulation =
            Planet.organisms.Count;

        if (currentPopulation >=
            previousPopulation + 5)
        {
            SimulationEvents.Add(
                "Популяция увеличилась: " +
                currentPopulation
            );

            previousPopulation =
                currentPopulation;
        }
        else if (
            currentPopulation <=
            previousPopulation - 5)
        {
            SimulationEvents.Add(
                "Попуяция уменьшилась: " +
                currentPopulation
            );

            previousPopulation =
                currentPopulation;
        }
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