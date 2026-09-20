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

    private void Start()
    {
        // Подписываем слайдеры и инициализируем текст в Start,
        // чтобы PlanetParameters успел выставить свои min/max значения
        foreach (var metric in metrics)
        {
            if (metric.slider != null && metric.percentText != null)
            {
                var currentMetric = metric;

                // Слушатель изменения ползунка
                currentMetric.slider.onValueChanged.AddListener((val) =>
                {
                    UpdateMetricText(currentMetric);
                });

                // Первичный вывод процентов при старте
                UpdateMetricText(currentMetric);
            }
        }
    }

    private void UpdateMetricText(MetricRow metric)
    {
        // Вычисляем процент заполнения шкалы от 0 до 1 независимо от min и max
        float normalized = Mathf.InverseLerp(metric.slider.minValue, metric.slider.maxValue, metric.slider.value);
        int percentage = Mathf.RoundToInt(normalized * 100f);

        metric.percentText.text = $"{percentage}%";
    }

    // Метод на случай принудительного обновления из других скриптов
    public void RefreshAll()
    {
        if (metrics == null) return;
        foreach (var metric in metrics)
        {
            UpdateMetricText(metric);
        }
    }
}