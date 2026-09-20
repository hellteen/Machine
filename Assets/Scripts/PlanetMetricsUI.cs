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
        foreach (var metric in metrics)
        {
            if (metric.slider != null && metric.percentText != null)
            {
                var currentMetric = metric;

                currentMetric.slider.onValueChanged.AddListener((val) =>
                {
                    UpdateMetricText(currentMetric);
                });

                UpdateMetricText(currentMetric);
            }
        }
    }

    private void UpdateMetricText(MetricRow metric)
    {
        float normalized = Mathf.InverseLerp(metric.slider.minValue, metric.slider.maxValue, metric.slider.value);
        int percentage = Mathf.RoundToInt(normalized * 100f);

        metric.percentText.text = $"{percentage}%";
    }

    public void RefreshAll()
    {
        if (metrics == null) return;
        foreach (var metric in metrics)
        {
            UpdateMetricText(metric);
        }
    }
}