using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EvolutionSystem
{
    public GenomeData CreateRandomGenome()
    {
        return new GenomeData
        {
            movement = Random.value,

            resourceSeeking = Random.value,

            reproduction = Random.value,

            waiting = Random.value,

            radiationResistance = Random.value,

            coldResistance = Random.value,

            exploration = Random.value
        };
    }

    public OrganismData CreateOffspring(
        OrganismData parent)
    {
        OrganismData child =
            new OrganismData();

        child.id =
            Random.Range(100000, 999999);

        child.position =
            new PositionData
            {
                x = parent.position.x +
                    Random.Range(-2f, 2f),

                y = parent.position.y,

                z = parent.position.z +
                    Random.Range(-2f, 2f)
            };

        Vector2 randomDirection =
            UnityEngine.Random.insideUnitCircle.normalized;

        child.direction =
            new DirectionData
            {
                x = randomDirection.x,
                y = randomDirection.y
            };

        child.state =
            new StateData
            {
                health = 100f,
                energy = 50f,
                age = 0f,
                hunger = 0f
            };

        child.genome =
            new GenomeData
            {
                movement =
                    Mutate(parent.genome.movement),

                resourceSeeking =
                    Mutate(parent.genome.resourceSeeking),

                reproduction =
                    Mutate(parent.genome.reproduction),

                waiting =
                    Mutate(parent.genome.waiting),

                radiationResistance =
                    Mutate(
                        parent.genome.radiationResistance
                    ),

                coldResistance =
                    Mutate(
                        parent.genome.coldResistance
                    ),

                exploration =
                    Mutate(parent.genome.exploration)
            };

        child.sensors =
            new SensorData
            {
                visionRange =
                    Mutate(
                        parent.sensors.visionRange
                    ),

                temperatureSensitivity =
                    Mutate(
                        parent.sensors
                            .temperatureSensitivity
                    ),

                radiationSensitivity =
                    Mutate(
                        parent.sensors
                            .radiationSensitivity
                    ),

                resourceSensitivity =
                    Mutate(
                        parent.sensors
                            .resourceSensitivity
                    )
            };

        child.memory =
            new MemoryData
            {
                capacity =
                    parent.memory.capacity
            };

        return child;
    }

    private float Mutate(float value)
    {
        float mutation =
            Random.Range(-0.15f, 0.15f);

        return Mathf.Clamp01(
            value + mutation
        );
    }
}