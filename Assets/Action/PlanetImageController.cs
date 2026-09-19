using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetImageController : MonoBehaviour
{
    [Serializable]
    public class PlanetVisualData
    {
        [Tooltip("JSON этой планеты")]
        public TextAsset planetJson;

        [Tooltip("Спрайт этой планеты")]
        public Sprite planetSprite;
    }

    [Header("Куда выводить картинку")]
    [SerializeField] private SpriteRenderer targetSpriteRenderer;

    [Header("Список планет")]
    [SerializeField] private List<PlanetVisualData> planets = new List<PlanetVisualData>();

    [Header("Тест в редакторе")]
    [SerializeField] private TextAsset defaultTestJson;

    private void Start()
    {
        ApplyPlanetVisual();
    }

    public void ApplyPlanetVisual()
    {
        TextAsset selectedJson = PlanetSelectionManager.SelectedPlanetJson;

        if (selectedJson == null)
        {
            selectedJson = defaultTestJson;
        }

        if (selectedJson == null || targetSpriteRenderer == null) return;

        foreach (var planet in planets)
        {
            if (planet.planetJson != null &&
               (planet.planetJson == selectedJson || planet.planetJson.name == selectedJson.name))
            {
                targetSpriteRenderer.sprite = planet.planetSprite;
                return;
            }
        }
    }
}