using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MoveDirection {

    Left, Right, Up, Down
}

// 조이스틱 클래스
public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform joyRect;
    private Vector2 joyPosition; // 중심 좌표
    private PointerEventData pointerEventData;

    [SerializeField] private Image img_arrowRight;
    [SerializeField] private Image img_arrowLeft;
    [SerializeField] private Image img_arrowUp;
    [SerializeField] private Image img_arrowDown;
    private readonly Color defaultJoyColor = new Color(1, 1, 1, 0.6f);
    private readonly Color selectJoyColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

    private Player localPlayer;

    private void Awake()
    {
        joyRect = GetComponent<RectTransform>();

        joyPosition = joyRect.anchoredPosition;
        joyPosition += new Vector2(joyRect.rect.width / 2, joyRect.rect.height / 2);
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

        Vector2 pointerPosInJoy = pointerPos - joyPosition;

        //Debug.Log(pointerPosInJoy);
        // y = x 와 y = -x 기준으로 영역을 나눔
        if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            localPlayer.MoveByJoystick(MoveDirection.Right); // 오른쪽
            SetArrowColor(img_arrowRight);
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            localPlayer.MoveByJoystick(MoveDirection.Left); // 왼쪽
            SetArrowColor(img_arrowLeft);
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            localPlayer.MoveByJoystick(MoveDirection.Up); // 위쪽
            SetArrowColor(img_arrowUp);
        }
        else if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            localPlayer.MoveByJoystick(MoveDirection.Down); // 아래쪽
            SetArrowColor(img_arrowDown);
        }
    }

    private void SetArrowColor(Image setArrow = null)
    {
        img_arrowRight.color = defaultJoyColor;
        img_arrowLeft.color = defaultJoyColor;
        img_arrowUp.color = defaultJoyColor;
        img_arrowDown.color = defaultJoyColor;

        if (setArrow != null)
            setArrow.color = selectJoyColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetArrowColor();
        pointerEventData = null;
    }
}