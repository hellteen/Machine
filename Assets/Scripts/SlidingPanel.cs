using System.Collections;
using UnityEngine;

public class SlidingTabPanel : MonoBehaviour
{
    public enum SlideSide { Left, Right }

    [Header("Направление")]
    [SerializeField] private SlideSide hideSide = SlideSide.Left;

    [Header("Дистанция уезда (в пикселях)")]
    [Tooltip("Если 0 — посчитает автоматически по ширине карточки. Если не двигается — вбей сюда 450 вручную!")]
    [SerializeField] private float customDistance = 0f;

    [Header("Настройки")]
    [SerializeField] private float slideDuration = 0.25f;
    [SerializeField] private RectTransform arrowIcon;

    private RectTransform panelRect;
    private Vector2 openedPos;
    private Vector2 closedPos;
    private bool isOpened = true;
    private Coroutine slideRoutine;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
        openedPos = panelRect.anchoredPosition;

        RecalculatePositions();
    }

    public void RecalculatePositions()
    {
        float dist = customDistance > 0f ? customDistance : panelRect.rect.width;
        if (dist <= 10f) dist = 450f; // Защита, если Unity посчитал ширину равной 0

        float dir = (hideSide == SlideSide.Left) ? -1f : 1f;
        closedPos = openedPos + new Vector2(dist * dir, 0f);
    }

    // Позволяет протестировать уезд прямо из меню скрипта по правому клику мыши
    [ContextMenu("Test Toggle (Проверить сдвиг)")]
    public void TogglePanel()
    {
        Debug.Log($"[SlidingTabPanel] Toggle вызван! Текущее состояние: открыто = {isOpened}");

        isOpened = !isOpened;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        Vector2 target = isOpened ? openedPos : closedPos;
        slideRoutine = StartCoroutine(SlideRoutine(target, isOpened));
    }

    private IEnumerator SlideRoutine(Vector2 targetPos, bool openState)
    {
        Vector2 startPos = panelRect.anchoredPosition;
        float elapsed = 0f;

        Quaternion startRot = (arrowIcon != null) ? arrowIcon.localRotation : Quaternion.identity;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, openState ? 0f : 180f);

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            panelRect.anchoredPosition = Vector2.LerpUnclamped(startPos, targetPos, smoothT);

            if (arrowIcon != null)
                arrowIcon.localRotation = Quaternion.Slerp(startRot, targetRot, smoothT);

            yield return null;
        }

        panelRect.anchoredPosition = targetPos;
        if (arrowIcon != null) arrowIcon.localRotation = targetRot;
        slideRoutine = null;
    }
}