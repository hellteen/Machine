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
        GenomeData genome =
            organism.genome;

        StateData state =
            organism.state;

        float moveScore =
            Random.Range(0f, 1f);

        float searchScore =
            Random.Range(0f, 1f);

        float waitScore =
            Random.Range(0f, 1f);

        float reproduceScore =
            Random.Range(0f, 1f);

        moveScore +=
            genome.movement *
            genome.exploration;

        searchScore +=
            genome.resourceSeeking;

        waitScore +=
            genome.waiting;

        reproduceScore +=
            genome.reproduction;

        searchScore +=
            state.hunger / 100f;

        waitScore +=
            (1f - state.energy / 100f);

        moveScore +=
            environment.radiation *
            (1f - genome.radiationResistance);

        searchScore +=
            environment.resources / 1000f *
            genome.resourceSeeking;

        moveScore +=
            Random.Range(-0.3f, 0.3f);

        searchScore +=
            Random.Range(-0.3f, 0.3f);

        waitScore +=
            Random.Range(-0.3f, 0.3f);

        reproduceScore +=
            Random.Range(-0.3f, 0.3f);

        OrganismAction action =
            OrganismAction.Move;

        float maxScore =
            moveScore;

        if (searchScore > maxScore)
        {
            maxScore = searchScore;

            action =
                OrganismAction.SearchResource;
        }

        if (waitScore > maxScore)
        {
            maxScore = waitScore;

            action =
                OrganismAction.Wait;
        }

        if (reproduceScore > maxScore)
        {
            maxScore =
                reproduceScore;

            action =
                OrganismAction.Reproduce;
        }

        return new DecisionResult
        {
            action = action,

            moveScore = moveScore,

            searchScore = searchScore,

            waitScore = waitScore,

            reproduceScore =
                reproduceScore,

            reason =
                "Action received the highest score"
        };
    }
}