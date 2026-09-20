using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum OrganismAction
{
    Move,
    SearchResource,
    Wait,
    Reproduce
}

public class DecisionResult
{
    public OrganismAction action;

    public float moveScore;
    public float searchScore;
    public float waitScore;
    public float reproduceScore;

    public string reason;
}

public class DecisionEngine
{
    public DecisionResult MakeDecision(
     OrganismData organism,
     EnvironmentData environment)
    {
        GenomeData genome = organism.genome;
        StateData state = organism.state;


        float moveScore =
            genome.movement * 1.5f +
            genome.exploration +
            environment.radiation *
            (1f - genome.radiationResistance);


        float searchScore =
            genome.resourceSeeking * 2f +
            state.hunger / 100f +
            environment.resources / 1000f;


        float waitScore =
            genome.waiting +
            (1f - state.energy / 100f);


        float reproduceScore =
            genome.reproduction *
            (state.energy / 100f);


        // небольшие мутации поведения
        moveScore += Random.Range(-0.2f, 0.2f);
        searchScore += Random.Range(-0.2f, 0.2f);
        waitScore += Random.Range(-0.2f, 0.2f);
        reproduceScore += Random.Range(-0.2f, 0.2f);


        float total =
            moveScore +
            searchScore +
            waitScore +
            reproduceScore;


        float choice =
            Random.Range(0, total);


        OrganismAction action;


        if (choice < moveScore)
            action = OrganismAction.Move;

        else if (choice < moveScore + searchScore)
            action = OrganismAction.SearchResource;

        else if (choice < moveScore + searchScore + waitScore)
            action = OrganismAction.Wait;

        else
            action = OrganismAction.Reproduce;



        return new DecisionResult
        {
            action = action,

            moveScore = moveScore,
            searchScore = searchScore,
            waitScore = waitScore,
            reproduceScore = reproduceScore,

            reason =
            "Action selected by adaptive scoring system"
        };
    }
}
