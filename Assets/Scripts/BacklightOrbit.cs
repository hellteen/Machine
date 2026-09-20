using UnityEngine;

[RequireComponent(typeof(Light))]
public class BacklightOrbit : MonoBehaviour
{
    [Header("Центр орбиты")]
    [Tooltip("Объект, вокруг которого летаем (перетащи сюда Planet_Pivot). Если пусто, будет точка 0,0,0")]
    public Transform targetCenter;

    [Header("Параметры полета")]
    [Tooltip("Расстояние от центра планет до света")]
    public float orbitRadius = 25f;
    [Tooltip("Скорость полета вокруг планет")]
    public float orbitSpeed = 1.5f;
    [Tooltip("Высота света над планетами")]
    public float height = 2f;

    [Header("Мягкая пульсация")]
    public float baseIntensity = 1.85f;
    public float pulseAmplitude = 0.4f;
    public float pulseSpeed = 1.5f;

    private Light targetLight;
    private float currentAngle = 0f;

    private void Awake()
    {
        targetLight = GetComponent<Light>();
    }

    private void Update()
    {
        Vector3 center = targetCenter != null ? targetCenter.position : Vector3.zero;

        currentAngle += orbitSpeed * Time.deltaTime;

        float x = center.x + Mathf.Sin(currentAngle) * orbitRadius;
        float z = center.z + Mathf.Cos(currentAngle) * orbitRadius;
        float y = center.y + height;

        transform.position = new Vector3(x, y, z);

        transform.LookAt(center);

        if (targetLight != null)
        {
            float wave = Mathf.Sin(Time.time * pulseSpeed);
            targetLight.intensity = baseIntensity + (wave * pulseAmplitude);
        }
    }
}