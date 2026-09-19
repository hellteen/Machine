using System.Collections;
using UnityEngine;
using TMPro;

public class PlanetCarousel : MonoBehaviour
{
    [System.Serializable]
    public struct PlanetStats
    {
        public string line1;
        public string line2;
        public string line3;

        public PlanetStats(string l1, string l2, string l3)
        {
            line1 = l1;
            line2 = l2;
            line3 = l3;
        }
    }

    [Header("UI Ссылки (Куда выводить текст)")]
    [SerializeField] private TMP_Text planetNameText;
    [Tooltip("Перетащите сюда 1-ю строчку текста из карточки")]
    public TMP_Text descText1;
    [Tooltip("Перетащите сюда 2-ю строчку текста из карточки")]
    public TMP_Text descText2;
    [Tooltip("Перетащите сюда 3-ю строчку текста из карточки")]
    public TMP_Text descText3;

    [Header("Объекты планет")]
    [Tooltip("Перетащите сюда 4 планеты: Юпитер, Уран, Марс, Нептун")]
    [SerializeField] private Transform[] planets;

    [Header("Настройки карусели")]
    [SerializeField] private float rotationDuration = 0.7f;
    [SerializeField] private float stepAngle = 90f;

    [Header("Анимация выбранной планеты")]
    [Tooltip("Скорость собственного вращения планеты вокруг оси")]
    [SerializeField] private float selfSpinSpeed = 25f;

    [Tooltip("Масштаб активной планеты (для легкого акцента)")]
    [SerializeField] private float selectedScaleMultiplier = 1.15f;
    [SerializeField] private float scaleChangeSpeed = 5f;

    [Header("Список названий")]
    [SerializeField]
    private string[] planetNames = new string[]
    {
        "Юпитер",
        "Уран",
        "Марс",
        "Нептун"
    };

    [Header("Характеристики для каждой планеты")]
   
    private PlanetStats[] planetStats = new PlanetStats[]
    {
        new PlanetStats("ВЫСОКОЕ ДАВЛЕНИЕ", "СИЛЬНАЯ РАДИАЦИЯ", "ТЕМПЕРАТУРА: -110 °C"),
        new PlanetStats("СИЛЬНОЕ ДАВЛЕНИЕ", "ОГРАНИЧЕННЫЕ РЕСУРСЫ",  "ТЕМПЕРАТУРА: -195 °C"),
        new PlanetStats("НИЗКОЕ ДАВЛЕНИЕ",      "ОГРАНИЧЕННЫЕ РЕСУРСЫ",  "ТЕМПЕРАТУРА: -63 °C"),
        new PlanetStats("НЕОБЫЧНЫЕ ПРИРОДНЫЕ УСЛОВИЯ", "УМЕРЕННЫЕ РЕСУРСЫ", "ТЕМПЕРАТУРА: -201 °C")
    };

    public static string SelectedPlanetName { get; private set; } = "Юпитер";

    private int currentIndex = 0;
    private bool isRotating = false;
    private float targetYAngle = 0f;
    private Vector3[] initialScales;

    private void Start()
    {
        targetYAngle = transform.eulerAngles.y;

        if (planets != null && planets.Length > 0)
        {
            initialScales = new Vector3[planets.Length];
            for (int i = 0; i < planets.Length; i++)
            {
                if (planets[i] != null)
                {
                    initialScales[i] = planets[i].localScale;
                }
            }
        }

        UpdatePlanetUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            RotateLeft();
        }

        if (planets == null || planets.Length == 0) return;

        // Вращаем центральную выбранную планету
        if (planets[currentIndex] != null)
        {
            planets[currentIndex].Rotate(Vector3.up, selfSpinSpeed * Time.deltaTime, Space.World);
        }

        // Плавно приближаем выбранную планету по размеру
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i] == null) continue;

            Vector3 targetScale = (i == currentIndex)
                ? initialScales[i] * selectedScaleMultiplier
                : initialScales[i];

            planets[i].localScale = Vector3.Lerp(planets[i].localScale, targetScale, Time.deltaTime * scaleChangeSpeed);
        }
    }

    public void RotateRight()
    {
        if (isRotating) return;

        targetYAngle -= stepAngle;
        currentIndex = (currentIndex + 1) % planetNames.Length;
        UpdatePlanetUI();

        StartCoroutine(AnimateRotation());
    }

    public void RotateLeft()
    {
        if (isRotating) return;

        targetYAngle += stepAngle;
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = planetNames.Length - 1;
        }
        UpdatePlanetUI();

        StartCoroutine(AnimateRotation());
    }

    private void UpdatePlanetUI()
    {
        SelectedPlanetName = planetNames[currentIndex];

        if (planetNameText != null)
        {
            planetNameText.text = SelectedPlanetName;
        }

        // Обновляем 3 поля характеристик
        if (planetStats != null && currentIndex < planetStats.Length)
        {
            if (descText1 != null) descText1.text = planetStats[currentIndex].line1;
            if (descText2 != null) descText2.text = planetStats[currentIndex].line2;
            if (descText3 != null) descText3.text = planetStats[currentIndex].line3;
        }
    }

    private IEnumerator AnimateRotation()
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, targetYAngle, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}