using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Header("Настройки смещения")]
    [Tooltip("Для 3D объекта/Quad ставь 0.2 - 0.4! Для UI Image внутри Canvas ставь 10 - 20.")]
    [SerializeField] private float maxOffset = 0.3f;

    [Tooltip("Насколько плавно и лениво догоняет курсор")]
    [SerializeField] private float smoothSpeed = 2f;

    [Tooltip("Инвертировать (фон плывет слегка в противоположную сторону)")]
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
            if (maxOffset < 1f) maxOffset = 15f;
        }
        else
        {
            startLocalPos = transform.localPosition;
            if (maxOffset > 2f) maxOffset = 0.3f;
        }
    }

    private void Update()
    {
        float mouseX = Mathf.Clamp((Input.mousePosition.x / Screen.width) * 2f - 1f, -1f, 1f);
        float mouseY = Mathf.Clamp((Input.mousePosition.y / Screen.height) * 2f - 1f, -1f, 1f);

        float dir = invert ? -1f : 1f;

        if (isUI)
        {
            Vector2 targetOffset = new Vector2(mouseX, mouseY) * (maxOffset * dir);
            Vector2 targetPos = startAnchoredPos + targetOffset;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.deltaTime * smoothSpeed);
        }
        else
        {
            Vector3 targetOffset = new Vector3(mouseX, mouseY, 0f) * (maxOffset * dir);
            Vector3 targetPos = startLocalPos + targetOffset;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
        }
    }
}