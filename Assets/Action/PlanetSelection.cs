
using UnityEngine;

public class PlanetSelection : MonoBehaviour
{
	[Header("JSON этой планеты")]
	[SerializeField]
	private TextAsset planetJson;

	public TextAsset PlanetJson
	{
		get { return planetJson; }
	}
}
```
