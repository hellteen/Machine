using UnityEngine;

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

                // Если организм захотел размножиться,
                // но недостаточно сил,
                // он просто ждёт.
                Wait(
                    organism,
                    environment
                );

                return false;
        }

        return false;
    }


    // -----------------------------------------
    // ДВИЖЕНИЕ
    // -----------------------------------------

    private void Move(
        OrganismData organism,
        EnvironmentData environment)
    {
        organism.state.energy -= 5f;

        organism.state.hunger += 4f;

        organism.state.age += 1f;


        // Радиация наносит повреждения.
        // Устойчивость уменьшает урон.
        float radiationDamage =
            environment.radiation *
            (1f -
             organism.genome.radiationResistance)
            * 4f;


        // Холод наносит повреждения.
        float coldLevel =
            Mathf.Max(
                0f,
                (-environment.temperature - 50f)
                / 100f
            );


        float coldDamage =
            coldLevel *
            (1f -
             organism.genome.coldResistance);


        organism.state.health -=
            radiationDamage +
            coldDamage;


        ClampState(organism);
    }


    // -----------------------------------------
    // ПОИСК РЕСУРСОВ
    // -----------------------------------------

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


    // -----------------------------------------
    // ОЖИДАНИЕ
    // -----------------------------------------

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


    // -----------------------------------------
    // ПРОВЕРКА РАЗМНОЖЕНИЯ
    // -----------------------------------------

    private bool CanReproduce(
        OrganismData organism)
    {
        return
            organism.state.energy >= 60f &&
            organism.state.health >= 50f &&
            organism.state.age >= 10f;
    }


    // -----------------------------------------
    // РАЗМНОЖЕНИЕ
    // -----------------------------------------

    private void Reproduce(
        OrganismData organism)
    {
        organism.state.energy -= 30f;

        organism.state.age += 1f;

        ClampState(organism);
    }


    // -----------------------------------------
    // ОГРАНИЧЕНИЕ ЗНАЧЕНИЙ
    // -----------------------------------------

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