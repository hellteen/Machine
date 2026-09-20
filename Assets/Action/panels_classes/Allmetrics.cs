using UnityEngine;
using TMPro;

public class Allmetrics : MonoBehaviour
{
    public TMP_Text text;


    void Update()
    {
        OrganismView[] organisms =
            FindObjectsOfType<OrganismView>();


        int population = organisms.Length;


        float totalEnergy = 0;
        float totalEfficiency = 0;


        float averageSpeed = 0;


        foreach (var organism in organisms)
        {
            OrganismData data = organism.Data;


            totalEnergy += data.state.energy;


            totalEfficiency +=
                data.genome.resourceSeeking +
                data.genome.reproduction;


            averageSpeed +=
                data.genome.movement;
        }


        float power = 0;
        float efficiency = 0;
        float entropy = 0;


        if (population > 0)
        {
            power =
                totalEnergy / population;


            efficiency =
                totalEfficiency / population;


            averageSpeed /=
                population;


            entropy =
                CalculateEntropy(organisms);
        }



        text.text =
            $"SYSTEM METRICS\n\n" +
            $"Population: {population}\n" +
            $"Power: {power:F1}\n" +
            $"Efficiency: {efficiency:F2}\n" +
            $"Entropy: {entropy:F2}";
    }



    float CalculateEntropy(
        OrganismView[] organisms)
    {
        float difference = 0;


        foreach (var organism in organisms)
        {
            difference +=
                organism.Data.genome.movement +
                organism.Data.genome.exploration +
                organism.Data.genome.resourceSeeking;
        }


        return difference /
            Mathf.Max(1, organisms.Length);
    }
}