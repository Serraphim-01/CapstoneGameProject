using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 input = Vector2.zero;
    private Vector2 startPos;
    private float radius;

    public Vector2 InputDirection => input;

    void Start()
    {
        #if UNITY_STANDALONE || UNITY_WEBGL
    gameObject.SetActive(false);
#endif

        radius = background.sizeDelta.x * 0.5f;
        background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(true);
        startPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - startPos;
        input = Vector2.ClampMagnitude(pos / radius, 1f);
        handle.anchoredPosition = input * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }
}
