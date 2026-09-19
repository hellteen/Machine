using System.Collections;
using UnityEngine;
using TMPro;

public class PlanetCarousel : MonoBehaviour
{
    [Header("UI Ссылки")]
    [SerializeField] private TMP_Text planetNameText;

    [Header("Объекты планет")]
    [Tooltip("Перетащите сюда 4 планеты в том же порядке: Юпитер, Уран, Марс, Нептун")]
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

    public static string SelectedPlanetName { get; private set; } = "Юпитер";

    private int currentIndex = 0;
    private bool isRotating = false;
    private float targetYAngle = 0f;
    private Vector3[] initialScales;

    private void Start()
    {
        targetYAngle = transform.eulerAngles.y;

        // Сохраняем исходные размеры моделей
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

        // 1. Вращаем только выбранную центральную планету вокруг своей оси
        if (planets[currentIndex] != null)
        {
            planets[currentIndex].Rotate(Vector3.up, selfSpinSpeed * Time.deltaTime, Space.World);
        }

        // 2. Плавно приближаем выбранную планету по размеру, а остальные держим в обычном масштабе
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