using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
	// JSON с начальными условиями планеты.
	[SerializeField]
	private TextAsset planetJson;


	// Интервал между шагами симуляции.
	// 1 = один тик в секунду.
	[SerializeField]
	private float tickInterval = 1f;


	// Текущая планета.
	public PlanetData Planet
	{
		get;
		private set;
	}


	// Последнее принятое решение.
	public DecisionResult LastDecision
	{
		get;
		private set;
	}


	private DecisionEngine decisionEngine;

	private ActionSystem actionSystem;

	private EvolutionSystem evolutionSystem;


	private float tickTimer;


	// -----------------------------------------
	// START
	// -----------------------------------------

	private void Start()
	{
		decisionEngine =
			new DecisionEngine();

		actionSystem =
			new ActionSystem();

		evolutionSystem =
			new EvolutionSystem();


		LoadPlanet();

		InitializePopulation();
	}


	// -----------------------------------------
	// UPDATE
	// -----------------------------------------

	private void Update()
	{
		tickTimer +=
			Time.deltaTime;


		if (tickTimer >= tickInterval)
		{
			tickTimer = 0f;

			SimulationTick();
		}
	}


	// -----------------------------------------
	// ЗАГРУЗКА JSON
	// -----------------------------------------

	private void LoadPlanet()
	{
		Planet =
			JsonUtility.FromJson<PlanetData>(
				planetJson.text
			);


		Debug.Log(
			"Loaded planet: " +
			Planet.name
		);
	}


	// -----------------------------------------
	// НАСТРОЙКА ПОПУЛЯЦИИ
	// -----------------------------------------

	private void InitializePopulation()
	{
		if (Planet.organisms == null)
		{
			Planet.organisms =
				new List<OrganismData>();
		}


		// У стартовых организмов
		// ещё нет генома в JSON.
		//
		// Поэтому создаём его случайно.

		foreach (
			OrganismData organism
			in Planet.organisms)
		{
			if (organism.genome == null)
			{
				organism.genome =
					evolutionSystem
						.CreateRandomGenome();
			}
		}
	}


	// -----------------------------------------
	// ОДИН ШАГ СИМУЛЯЦИИ
	// -----------------------------------------

	private void SimulationTick()
	{
		if (
			Planet == null ||
			Planet.organisms == null
		)
		{
			return;
		}


		// Сюда складываем новых организмов.
		List<OrganismData> newborns =
			new List<OrganismData>();


		// Работаем с копией списка.
		// Это нужно потому, что во время
		// симуляции могут появиться новые организмы.

		List<OrganismData> population =
			new List<OrganismData>(
				Planet.organisms
			);


		// -------------------------------------
		// ОБРАБОТКА ВСЕХ ОРГАНИЗМОВ
		// -------------------------------------

		foreach (
			OrganismData organism
			in population)
		{
			// Мёртвые организмы ничего
			// больше не делают.
			if (
				organism.state.health <= 0)
			{
				continue;
			}


			// Организм принимает решение.
			LastDecision =
				decisionEngine.MakeDecision(
					organism,
					Planet.environment
				);


			// Выполняем решение.
			bool reproduced =
				actionSystem.Execute(
					organism,
					Planet.environment,
					LastDecision
				);


			// Если размножился —
			// создаём потомка.
			if (reproduced)
			{
				OrganismData child =
					evolutionSystem
						.CreateOffspring(
							organism
						);


				newborns.Add(child);


				Debug.Log(
					"Organism " +
					organism.id +
					" reproduced. " +
					"Child: " +
					child.id
				);
			}
		}


		// -------------------------------------
		// ДОБАВЛЯЕМ НОВОРОЖДЁННЫХ
		// -------------------------------------

		Planet.organisms.AddRange(
			newborns
		);


		// -------------------------------------
		// УДАЛЯЕМ ПОГИБШИХ
		// -------------------------------------

		RemoveDeadOrganisms();


		// -------------------------------------
		// ВОССТАНАВЛИВАЕМ РЕСУРСЫ
		// -------------------------------------

		UpdateEnvironment();


		Debug.Log(
			"Population: " +
			Planet.organisms.Count
		);
	}


	// -----------------------------------------
	// УДАЛЕНИЕ МЁРТВЫХ
	// -----------------------------------------

	private void RemoveDeadOrganisms()
	{
		Planet.organisms.RemoveAll(
			organism =>
				organism.state.health <= 0
		);
	}


	// -----------------------------------------
	// ИЗМЕНЕНИЕ СРЕДЫ
	// -----------------------------------------

	private void UpdateEnvironment()
	{
		Planet.environment.resources +=
			Planet.environment
				.resourceRegeneration;


		Planet.environment.resources =
			Mathf.Clamp(
				Planet.environment.resources,
				0f,
				2000f
			);
	}
}