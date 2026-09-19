using UnityEngine;
using Random = UnityEngine.Random;

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
        // Немного меняем направление,
        // чтобы движение было хаотичным.
        organism.direction.x +=
            Random.Range(-0.3f, 0.3f);

        organism.direction.y +=
            Random.Range(-0.3f, 0.3f);

        // Нормализуем направление.
        Vector2 direction =
            new Vector2(
                organism.direction.x,
                organism.direction.y
            ).normalized;

        organism.direction.x =
            direction.x;

        organism.direction.y =
            direction.y;


        // Скорость зависит от гена движения.
        float speed =
            0.2f +
            organism.genome.movement * 0.8f;


        // Двигаем организм.
        organism.position.x +=
            direction.x * speed;

        organism.position.y +=
            direction.y * speed;


        // Границы твоей панели.
        organism.position.x =
            Mathf.Clamp(
                organism.position.x,
                -980f,
                980f
            );

        organism.position.y =
            Mathf.Clamp(
                organism.position.y,
                -540f,
                540f
            );


        // Расход энергии.
        organism.state.energy -=
            1f + organism.genome.movement;

        organism.state.hunger += 2f;

        organism.state.age += 1f;


        // Урон от радиации.
        float radiationDamage =
            environment.radiation *
            (1f - organism.genome.radiationResistance)
            * 4f;

        organism.state.health -=
            radiationDamage;

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