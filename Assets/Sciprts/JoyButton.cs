using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform joyRect;
    private Vector2 joyPosition; // �߽� ��ǥ
    private PointerEventData pointerEventData;

    [SerializeField] private Image img_arrowRight;
    [SerializeField] private Image img_arrowLeft;
    [SerializeField] private Image img_arrowUp;
    [SerializeField] private Image img_arrowDown;

    private Player localPlayer;

    private void Awake()
    {
        joyRect = GetComponent<RectTransform>();
        // JoyButton�� �߽� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
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
        // JoyButton�� �߽��� (0, 0)���� ���� ���� ��� ��ǥ ���ϱ�
        Vector2 pointerPosInJoy = pointerPos - joyPosition;
        //Debug.Log(pointerPosInJoy);
        // y = x �� y = -x �������� ������ ����
        if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            //Debug.Log("������");
            localPlayer.MoveToJoy(1); // ������
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            //Debug.Log("����");
            localPlayer.MoveToJoy(2); // ����
        }
        else if (pointerPosInJoy.y > pointerPosInJoy.x && pointerPosInJoy.y > -pointerPosInJoy.x)
        {
            //Debug.Log("����");
            localPlayer.MoveToJoy(3); // ����
        }
        else if (pointerPosInJoy.y < pointerPosInJoy.x && pointerPosInJoy.y < -pointerPosInJoy.x)
        {
            //Debug.Log("�Ʒ���");
            localPlayer.MoveToJoy(4); // �Ʒ���
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerEventData = null;
    }
}