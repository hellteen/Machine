using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Effect_button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 orScale;
    private Vector3 tarScale;

    private Image btnImage;
    private Color orColor;
    private Color tarColor;

    [Header("Настройки анимации размера")]
    public float hoverScale = 1.05f;
    public float clickScale = 0.95f;
    public float speed = 12f;

    [Header("Настройки подсветки")]
    // Светло-голубой/неоновый оттенок по умолчанию (подходит под ледяной стиль)
    public Color hoverColor = new Color(0.7f, 0.95f, 1f, 1f);
    public Color clickColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    void Awake()
    {
        btnImage = GetComponent<Image>();
    }

    void Start()
    {
        orScale = transform.localScale;
        tarScale = orScale;

        if (btnImage != null)
        {
            orColor = btnImage.color;
            tarColor = orColor;
        }
    }

    void Update()
    {
        // Плавный зум
        transform.localScale = Vector3.Lerp(transform.localScale, tarScale, Time.deltaTime * speed);

        // Плавная подсветка
        if (btnImage != null)
        {
            btnImage.color = Color.Lerp(btnImage.color, tarColor, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tarScale = orScale * hoverScale;
        tarColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tarScale = orScale;
        tarColor = orColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tarScale = orScale * clickScale;
        tarColor = clickColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tarScale = orScale * hoverScale;
        tarColor = hoverColor;
    }
}