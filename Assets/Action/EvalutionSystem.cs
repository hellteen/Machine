using System;
using UnityEngine;

public class EvolutionSystem
{
	// -----------------------------------------
	// СОЗДАНИЕ СЛУЧАЙНОГО ГЕНОМА
	// -----------------------------------------

	public GenomeData CreateRandomGenome()
	{
		return new GenomeData
		{
			movement = Random.value,

			resourceSeeking = Random.value,

			reproduction = Random.value,

			waiting = Random.value,

			radiationResistance = Random.value,

			coldResistance = Random.value,

			exploration = Random.value
		};
	}


	// -----------------------------------------
	// СОЗДАНИЕ ПОТОМКА
	// -----------------------------------------

	public OrganismData CreateOffspring(
		OrganismData parent)
	{
		OrganismData child =
			new OrganismData();

		// У каждого организма свой ID.
		child.id =
			Random.Range(100000, 999999);


		// -------------------------------------
		// ПОЗИЦИЯ
		// -------------------------------------

		child.position =
			new PositionData
			{
				x = parent.position.x +
					Random.Range(-2f, 2f),

				y = parent.position.y,

				z = parent.position.z +
					Random.Range(-2f, 2f)
			};


		// -------------------------------------
		// НАЧАЛЬНОЕ СОСТОЯНИЕ
		// -------------------------------------

		child.state =
			new StateData
			{
				health = 100f,
				energy = 50f,
				age = 0f,
				hunger = 0f
			};


		// -------------------------------------
		// ГЕНЫ
		// -------------------------------------

		// Потомок получает гены родителя,
		// но каждый ген может немного измениться.
		child.genome =
			new GenomeData
			{
				movement =
					Mutate(parent.genome.movement),

				resourceSeeking =
					Mutate(parent.genome.resourceSeeking),

				reproduction =
					Mutate(parent.genome.reproduction),

				waiting =
					Mutate(parent.genome.waiting),

				radiationResistance =
					Mutate(
						parent.genome.radiationResistance
					),

				coldResistance =
					Mutate(
						parent.genome.coldResistance
					),

				exploration =
					Mutate(parent.genome.exploration)
			};


		// -------------------------------------
		// ДАТЧИКИ
		// -------------------------------------

		child.sensors =
			new SensorData
			{
				visionRange =
					Mutate(
						parent.sensors.visionRange
					),

				temperatureSensitivity =
					Mutate(
						parent.sensors
							.temperatureSensitivity
					),

				radiationSensitivity =
					Mutate(
						parent.sensors
							.radiationSensitivity
					),

				resourceSensitivity =
					Mutate(
						parent.sensors
							.resourceSensitivity
					)
			};


		// -------------------------------------
		// ПАМЯТЬ
		// -------------------------------------

		child.memory =
			new MemoryData
			{
				capacity =
					parent.memory.capacity
			};


		return child;
	}


	// -----------------------------------------
	// МУТАЦИЯ
	// -----------------------------------------

	private float Mutate(float value)
	{
		// Небольшое случайное изменение.
		float mutation =
			Random.Range(-0.15f, 0.15f);

		// Значение остаётся между 0 и 1.
		return Mathf.Clamp01(
			value + mutation
		);
	}
}