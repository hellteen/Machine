using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlanetClickEffect : MonoBehaviour
{
    [Header("Световой ореол сзади")]
    public Light backLight;
    public float baseLightIntensity = 4f;
    public float flashLightIntensity = 18f;

    [Header("Голографическое кольцо")]
    public Transform holoRing;
    public float ringRotateSpeed = 40f;

    [Header("Настройки отдачи (Punch)")]
    public float punchScaleAmount = 1.15f;
    public float punchDuration = 0.35f;

    private Vector3 baseScale;
    private Coroutine punchCoroutine;

    private void Start()
    {
        baseScale = transform.localScale;

        if (backLight != null)
        {
            backLight.intensity = baseLightIntensity;
        }
    }

    private void Update()
    {
        if (holoRing != null)
        {
            holoRing.Rotate(0f, 0f, ringRotateSpeed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        TriggerHighlight();
    }

    public void TriggerHighlight()
    {
        if (punchCoroutine != null) StopCoroutine(punchCoroutine);
        punchCoroutine = StartCoroutine(AnimatePunchAndFlash());
    }

    private IEnumerator AnimatePunchAndFlash()
    {
        float elapsed = 0f;

        while (elapsed < punchDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / punchDuration;

            float curve = Mathf.Sin(t * Mathf.PI);
            transform.localScale = baseScale * (1f + curve * (punchScaleAmount - 1f));

            if (backLight != null)
            {
                backLight.intensity = Mathf.Lerp(flashLightIntensity, baseLightIntensity, t);
            }

            yield return null;
        }

        transform.localScale = baseScale;
        if (backLight != null) backLight.intensity = baseLightIntensity;
    }
}