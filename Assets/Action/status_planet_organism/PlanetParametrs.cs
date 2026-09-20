using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlanetParameters : MonoBehaviour
{
    [Header("Planet Sliders")]
    [SerializeField] private Slider sliderWarm;
    [SerializeField] private Slider sliderRadias;
    [SerializeField] private Slider sliderResurs;
    [SerializeField] private Slider sliderEnergy;

    private PlanetData planet;
    public void Initialize(PlanetData data)
    {
        planet = data;

        Debug.Log(
            "PlanetParameters INITIALIZED: " +
            planet.name
        );

        if (planet == null)
        {
            Debug.LogError("PlanetParameters: planet == null");
            return;
        }

        SetupSliders();
        UpdateSliders();
        SubscribeToSliders();
    }

    //public void Initialize(PlanetData data)
    //{
    //    planet = data;

    //    if (planet == null)
    //    {
    //        return;
    //    }

    //    SetupSliders();
    //    UpdateSliders();
    //    SubscribeToSliders();
    //}

    private void SetupSliders()
    {
        sliderWarm.minValue = -200f;
        sliderWarm.maxValue = 200f;

        sliderRadias.minValue = 0f;
        sliderRadias.maxValue = 1f;

        sliderResurs.minValue = 0f;
        sliderResurs.maxValue = 2000f;

        sliderEnergy.minValue = 0f;
        sliderEnergy.maxValue = 2000f;
    }

    public void UpdateSliders()
    {
        sliderWarm.value =
            planet.environment.temperature;

        sliderRadias.value =
            planet.environment.radiation;

        sliderResurs.value =
            planet.environment.resources;

        sliderEnergy.value =
            planet.environment.energy;
    }

    private void SubscribeToSliders()
    {
        sliderWarm.onValueChanged.AddListener(
            OnTemperatureChanged
        );

        sliderRadias.onValueChanged.AddListener(
            OnRadiationChanged
        );

        sliderResurs.onValueChanged.AddListener(
            OnResourcesChanged
        );
    }

    private void OnTemperatureChanged(float value)
    {
        planet.environment.temperature = value;
    }

    private void OnRadiationChanged(float value)
    {
        planet.environment.radiation = value;
    }

    private void OnResourcesChanged(float value)
    {
        planet.environment.resources = value;
    }
}