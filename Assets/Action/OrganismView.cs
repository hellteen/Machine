using UnityEngine;

public class OrganismView : MonoBehaviour
{
	// Данные организма,
	// которому соответствует этот GameObject.
	public OrganismData Data
	{
		get;
		private set;
	}


	// -----------------------------------------
	// ПОДКЛЮЧЕНИЕ ДАННЫХ
	// -----------------------------------------

	public void Initialize(
		OrganismData data)
	{
		Data = data;

		UpdateView();
	}


	// -----------------------------------------
	// UPDATE
	// -----------------------------------------

	private void Update()
	{
		if (Data == null)
		{
			return;
		}

		UpdateView();
	}


	// -----------------------------------------
	// ОБНОВЛЕНИЕ ВИДА
	// -----------------------------------------

	private void UpdateView()
	{
		transform.position =
			new Vector3(
				Data.position.x,
				Data.position.y,
				Data.position.z
			);
	}
}