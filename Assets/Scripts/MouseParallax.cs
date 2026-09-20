using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Header("Настройки движения")]
    [Tooltip("Сила сдвига: для UI картинок ставь 30-60, для 3D/Quad ставь 0.5-1.5")]
    [SerializeField] private float intensity = 40f;

    [Tooltip("Тягучесть/плавность (чем меньше число, тем мягче и ленивее плывёт фон)")]
    [SerializeField] private float smoothSpeed = 3.5f;

    [Tooltip("Инверсия: фон идёт слегка в противоположную сторону от мыши (создаёт эффект глубины)")]
    [SerializeField] private bool invert = true;

    private RectTransform rectTransform;
    private Vector2 startAnchoredPos;
    private Vector3 startLocalPos;
    private bool isUI;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isUI = (rectTransform != null);

        if (isUI)
        {
            startAnchoredPos = rectTransform.anchoredPosition;
        }
        else
        {
            startLocalPos = transform.localPosition;
        }
    }

    private void Update()
    {
        // Нормализуем координаты курсора относительно центра экрана: от -0.5 до +0.5
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        float dir = invert ? -1f : 1f;

        if (isUI)
        {
            Vector2 target = startAnchoredPos + new Vector2(mouseX, mouseY) * (intensity * dir);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, Time.deltaTime * smoothSpeed);
        }
        else
        {
            Vector3 target = startLocalPos + new Vector3(mouseX, mouseY, 0f) * (intensity * dir);
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * smoothSpeed);
        }
    }
}