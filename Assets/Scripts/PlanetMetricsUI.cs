using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetMetricsUI : MonoBehaviour
{
    [System.Serializable]
    public class MetricRow
    {
        public string name;
        public Slider slider;
        public TMP_Text percentText;
    }

    [Header("Связки слайдеров и текстов")]
    [SerializeField] private MetricRow[] metrics;

    private void Awake()
    {
        // Подписываем каждый слайдер на автоматическое обновление своего текста
        foreach (var metric in metrics)
        {
            if (metric.slider != null && metric.percentText != null)
            {
                // Запоминаем локальную копию переменной для корректной работы лямбды
                var currentMetric = metric;

                currentMetric.slider.onValueChanged.AddListener((val) =>
                {
                    currentMetric.percentText.text = $"{Mathf.RoundToInt(val)}%";
                });

                // Первичная инициализация при старте сцены
                currentMetric.percentText.text = $"{Mathf.RoundToInt(currentMetric.slider.value)}%";
            }
        }
    }
}