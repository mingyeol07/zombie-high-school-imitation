using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform joyRect;
    private Vector2 joyPosition; // 중심 좌표
    private PointerEventData pointerEventData;

    [SerializeField] private Image img_arrowRight;
    [SerializeField] private Image img_arrowLeft;
    [SerializeField] private Image img_arrowUp;
    [SerializeField] private Image img_arrowDown;

    private Player localPlayer;

    private void Awake()
    {
        joyRect = GetComponent<RectTransform>();
        // JoyButton의 중심 좌표를 스크린 좌표로 변환
        joyPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, joyRect.position);

        Debug.Log(joyPosition);
    }

    private void Start()
    {
        localPlayer = GameManager.Instance.Player;
    }

    private void Update()
    {
        OnJoyPressed(pointerEventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerEventData = eventData;
    }

    public void OnJoyPressed(PointerEventData eventData)
    {
        if (eventData == null) return;

        Vector2 pointerPos = eventData.position;
        Debug.Log(pointerPos);
        Debug.Log(joyPosition);
        // JoyButton의 중심을 (0, 0)으로 기준 잡은 상대 좌표 구하기
        Vector2 pointerPosInJoy = pointerPos - joyPosition;
        //Debug.Log(pointerPosInJoy);
        // y = x 와 y = -x 기준으로 영역을 나눔
        if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            //Debug.Log("오른쪽");
            localPlayer.MoveToJoy(1); // 오른쪽
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            //Debug.Log("왼쪽");
            localPlayer.MoveToJoy(2); // 왼쪽
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            //Debug.Log("위쪽");
            localPlayer.MoveToJoy(3); // 위쪽
        }
        else if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            //Debug.Log("아래쪽");
            localPlayer.MoveToJoy(4); // 아래쪽
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerEventData = null;
    }
}