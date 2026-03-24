using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class AndroidCameraUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image CameraControlArea;
    [SerializeField] CinemachineFreeLook freeLook;

    private float lookSpeedX = 0.05f;
    private float lookSpeedY = 0.005f;

    void Start()
    {
        CameraControlArea = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CameraControlArea.rectTransform, 
            eventData.position, 
            eventData.enterEventCamera, 
            out Vector2 position))
        {
            //Debug.Log(position);
            freeLook.m_XAxis.m_InputAxisValue = eventData.delta.x * lookSpeedX;
            freeLook.m_YAxis.m_InputAxisValue = eventData.delta.y * lookSpeedY;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        freeLook.m_XAxis.m_InputAxisValue = 0;
        freeLook.m_YAxis.m_InputAxisValue = 0;
    }
}
