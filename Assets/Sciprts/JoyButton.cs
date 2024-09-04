using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.UIElements;

public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isPressed;
    [SerializeField] private RectTransform background;
    private float colliderDistanceY;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        OnDrag (eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        pos = eventData.position;

        pos.x = (pos.x / background.sizeDelta.x / 2);
        pos.y = (pos.y / background.sizeDelta.y / 2);

        Debug.Log(pos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
