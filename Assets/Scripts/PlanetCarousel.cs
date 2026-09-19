using System.Collections;
using UnityEngine;

public class PlanetCarousel : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Время анимации одного поворота (в секундах)")]
    [SerializeField] private float rotationDuration = 0.4f;

    [Tooltip("Шаг поворота в градусах (для 4 планет: 360 / 4 = 90)")]
    [SerializeField] private float stepAngle = 90f;

    private bool isRotating = false;
    private float targetYAngle = 0f;

    private void Start()
    {
        targetYAngle = transform.eulerAngles.y;
    }

    // Вызывается при клике на кнопку ">"
    public void RotateRight()
    {
        if (isRotating) return;
        targetYAngle -= stepAngle;
        StartCoroutine(AnimateRotation());
    }

    // Вызывается при клике на кнопку "<"
    public void RotateLeft()
    {
        if (isRotating) return;
        targetYAngle += stepAngle;
        StartCoroutine(AnimateRotation());
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

            // Плавный старт и мягкое торможение
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}