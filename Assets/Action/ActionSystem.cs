using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ActionSystem
{
    public bool Execute(
        OrganismData organism,
        EnvironmentData environment,
        DecisionResult decision)
    {
        switch (decision.action)
        {
            case OrganismAction.Move:

                Move(
                    organism,
                    environment
                );

                return false;


            case OrganismAction.SearchResource:

                SearchResource(
                    organism,
                    environment
                );

                return false;


            case OrganismAction.Wait:

                Wait(
                    organism,
                    environment
                );

                return false;


            case OrganismAction.Reproduce:

                if (CanReproduce(organism))
                {
                    Reproduce(organism);

                    return true;
                }

                Wait(
                    organism,
                    environment
                );

                return false;
        }

        return false;
    }

    private void Move(
     OrganismData organism,
     EnvironmentData environment)
    {
        Debug.Log(
    "MOVE " +
    organism.id +
    " position: " +
    organism.position.x +
    ", " +
    organism.position.y
);
        if (organism.direction == null)
        {
            Vector2 randomDirection =
                Random.insideUnitCircle.normalized;

            organism.direction = new DirectionData
            {
                x = randomDirection.x,
                y = randomDirection.y
            };
        }

        // Немного меняем направление
        organism.direction.x += Random.Range(-0.15f, 0.15f);
        organism.direction.y += Random.Range(-0.15f, 0.15f);

        Vector2 direction =
            new Vector2(
                organism.direction.x,
                organism.direction.y
            ).normalized;

        organism.direction.x = direction.x;
        organism.direction.y = direction.y;

        // Скорость
        float speed =
            0.5f + organism.genome.movement * 1f;
        // ДВИЖЕНИЕ
        organism.position.x +=
    direction.x * speed;

        organism.position.y +=
            direction.y * speed;

        //границы
        float minX = -6f;
        float maxX = 6f;

        float minY = -3f;
        float maxY = 3f;

        if (organism.position.x <= minX)
        {
            organism.position.x = minX;
            organism.direction.x =
                Mathf.Abs(organism.direction.x);
        }
        else if (organism.position.x >= maxX)
        {
            organism.position.x = maxX;
            organism.direction.x =
                -Mathf.Abs(organism.direction.x);
        }

        if (organism.position.y <= minY)
        {
            organism.position.y = minY;
            organism.direction.y =
                Mathf.Abs(organism.direction.y);
        }
        else if (organism.position.y >= maxY)
        {
            organism.position.y = maxY;
            organism.direction.y =
                -Mathf.Abs(organism.direction.y);
        }

        organism.state.energy -=
            1f + organism.genome.movement;

        organism.state.hunger += 2f;
        organism.state.age += 1f;

        float radiationDamage =
            environment.radiation *
            (1f - organism.genome.radiationResistance)
            * 4f;

        organism.state.health -= radiationDamage;

        ClampState(organism);
    }

    private void SearchResource(
        OrganismData organism,
        EnvironmentData environment)
    {
        if (environment.resources <= 0)
        {
            organism.state.hunger += 5f;

            organism.state.energy -= 2f;

            organism.state.age += 1f;

            ClampState(organism);

            return;
        }


        float collected =
            5f +
            organism.genome.resourceSeeking
            * 10f;


        collected =
            Mathf.Min(
                collected,
                environment.resources
            );


        environment.resources -=
            collected;


        organism.state.energy +=
            collected * 0.8f;


        organism.state.hunger -=
            collected * 0.7f;


        organism.state.age += 1f;


        ClampState(organism);
    }

    private void Wait(
        OrganismData organism,
        EnvironmentData environment)
    {
        organism.state.energy += 4f;

        organism.state.hunger += 3f;

        organism.state.age += 1f;


        float radiationDamage =
            environment.radiation *
            (1f -
             organism.genome.radiationResistance);


        organism.state.health -=
            radiationDamage;


        ClampState(organism);
    }
   

    private bool CanReproduce(
        OrganismData organism)
    {
        return
            organism.state.energy >= 60f &&
            organism.state.health >= 50f &&
            organism.state.age >= 10f;
    }

    private void Reproduce(
        OrganismData organism)
    {
        organism.state.energy -= 30f;

        organism.state.age += 1f;

        ClampState(organism);
    }

    private void ClampState(
        OrganismData organism)
    {
        organism.state.health =
            Mathf.Clamp(
                organism.state.health,
                0f,
                100f
            );


        organism.state.energy =
            Mathf.Clamp(
                organism.state.energy,
                0f,
                100f
            );


        organism.state.hunger =
            Mathf.Clamp(
                organism.state.hunger,
                0f,
                100f
            );
    }
}