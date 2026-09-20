using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PopulationStatus : MonoBehaviour
{
	[Header("Population Status")]
	[SerializeField] private Slider sliderEnergy;
	[SerializeField] private Slider sliderHP;
	[SerializeField] private Slider sliderAge;
	[SerializeField] private Slider sliderHungry;

	private PlanetData planet;

	public void Initialize(PlanetData data)
	{
		planet = data;

		Debug.Log(
			"PopulationStatus INITIALIZED: " +
			planet.name
		);

		if (planet == null)
		{
			Debug.LogError("PopulationStatus: planet == null");
			return;
		}

		SetupSliders();
		UpdateStats();
	}

	//public void Initialize(PlanetData data)
	//{
	//	planet = data;

	//	if (planet == null)
	//	{
	//		return;
	//	}

	//	SetupSliders();
	//	UpdateStats();
	//}

	private void SetupSliders()
	{
		sliderEnergy.minValue = 0f;
		sliderEnergy.maxValue = 100f;

		sliderHP.minValue = 0f;
		sliderHP.maxValue = 100f;

		sliderAge.minValue = 0f;
		sliderAge.maxValue = 100f;

		sliderHungry.minValue = 0f;
		sliderHungry.maxValue = 100f;
	}

	public void UpdateStats()
	{
		if (
			planet == null ||
			planet.organisms == null ||
			planet.organisms.Count == 0
		)
		{
			sliderEnergy.value = 0f;
			sliderHP.value = 0f;
			sliderAge.value = 0f;
			sliderHungry.value = 0f;

			return;
		}

		float totalEnergy = 0f;
		float totalHP = 0f;
		float totalAge = 0f;
		float totalHungry = 0f;

		foreach (OrganismData organism in planet.organisms)
		{
			totalEnergy += organism.state.energy;
			totalHP += organism.state.health;
			totalAge += organism.state.age;
			totalHungry += organism.state.hunger;
		}

		int population =
			planet.organisms.Count;

		sliderEnergy.value =
			totalEnergy / population;

		sliderHP.value =
			totalHP / population;

		sliderAge.value =
			totalAge / population;

		sliderHungry.value =
			totalHungry / population;
	}
}