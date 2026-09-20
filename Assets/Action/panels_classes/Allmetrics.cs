using UnityEngine;
using UnityEngine.UI;

public class Allmetrics : MonoBehaviour
{
    public Slider powerSlider;
    public Slider efficiencySlider;
    public Slider entropySlider;
    public Slider responseDelaySlider;

    public float Power { get; private set; }
    public float Efficiency { get; private set; }
    public float Entropy { get; private set; }
    public float ResponseDelay { get; private set; }

    void Start()
    {
        ResponseDelay = Random.Range(10f, 90f);
    }

    void Update()
    {
        OrganismView[] organisms =
            FindObjectsOfType<OrganismView>();

        int population = organisms.Length;

        float totalEnergy = 0;
        float totalEfficiency = 0;

        foreach (var organism in organisms)
        {
            OrganismData data = organism.Data;

            totalEnergy += data.state.energy;

            totalEfficiency +=
                data.genome.resourceSeeking +
                data.genome.reproduction;
        }

        Power = 0;
        Efficiency = 0;
        Entropy = 0;

        if (population > 0)
        {
            Power =
                totalEnergy / population;

            Efficiency =
                totalEfficiency / population;

            Entropy =
                CalculateEntropy(organisms);
        }

        // Обновляем UI
        powerSlider.value = Power;
        efficiencySlider.value = Efficiency;
        entropySlider.value = Entropy;

        // Новая задержка
        ResponseDelay =
            Random.Range(10f, 90f);

        responseDelaySlider.value =
            ResponseDelay;
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