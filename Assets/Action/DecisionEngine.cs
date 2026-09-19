using System;
using UnityEngine;
using Random = UnityEngine.Random;

// -----------------------------------------
// ВОЗМОЖНЫЕ ДЕЙСТВИЯ
// -----------------------------------------

public enum OrganismAction
{
	Move,
	SearchResource,
	Wait,
	Reproduce
}


// -----------------------------------------
// РЕЗУЛЬТАТ РЕШЕНИЯ
// -----------------------------------------

public class DecisionResult
{
	public OrganismAction action;

	public float moveScore;
	public float searchScore;
	public float waitScore;
	public float reproduceScore;

	public string reason;
}


// -----------------------------------------
// ДВИЖОК ПРИНЯТИЯ РЕШЕНИЙ
// -----------------------------------------

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


		// -------------------------------------
		// НАЧАЛЬНЫЕ СЛУЧАЙНЫЕ ЗНАЧЕНИЯ
		// -------------------------------------

		float moveScore =
			Random.Range(0f, 1f);

		float searchScore =
			Random.Range(0f, 1f);

		float waitScore =
			Random.Range(0f, 1f);

		float reproduceScore =
			Random.Range(0f, 1f);


		// -------------------------------------
		// ВЛИЯНИЕ ГЕНОВ
		// -------------------------------------

		moveScore +=
			genome.movement *
			genome.exploration;

		searchScore +=
			genome.resourceSeeking;

		waitScore +=
			genome.waiting;

		reproduceScore +=
			genome.reproduction;


		// -------------------------------------
		// ВЛИЯНИЕ СОСТОЯНИЯ
		// -------------------------------------

		// Чем выше голод,
		// тем больше вероятность поиска ресурсов.
		searchScore +=
			state.hunger / 100f;


		// Чем меньше энергии,
		// тем привлекательнее ожидание.
		waitScore +=
			(1f - state.energy / 100f);


		// -------------------------------------
		// ВЛИЯНИЕ СРЕДЫ
		// -------------------------------------

		// Высокая радиация делает движение
		// более привлекательным.
		moveScore +=
			environment.radiation *
			(1f - genome.radiationResistance);


		// Чем больше ресурсов,
		// тем интереснее их искать.
		searchScore +=
			environment.resources / 1000f *
			genome.resourceSeeking;


		// -------------------------------------
		// СЛУЧАЙНЫЙ ШУМ
		// -------------------------------------

		// Даже при одинаковых условиях
		// организм не обязан делать одно и то же.
		moveScore +=
			Random.Range(-0.3f, 0.3f);

		searchScore +=
			Random.Range(-0.3f, 0.3f);

		waitScore +=
			Random.Range(-0.3f, 0.3f);

		reproduceScore +=
			Random.Range(-0.3f, 0.3f);


		// -------------------------------------
		// ВЫБИРАЕМ ЛУЧШИЙ SCORE
		// -------------------------------------

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


		// -------------------------------------
		// РЕЗУЛЬТАТ
		// -------------------------------------

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