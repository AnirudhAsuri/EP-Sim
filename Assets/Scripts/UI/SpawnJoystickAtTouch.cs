using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class SpawnJoystickAtTouch : MonoBehaviour, IPointerDownHandler
{
    public RectTransform joystickBase; // The parent of the sprite
    public OnScreenStick stickComponent; // Drag the sprite with the component here

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        joystickBase.gameObject.SetActive(false); // Hide until touch
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickBase.gameObject.SetActive(true);

        // Position the Base exactly at the thumb
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        joystickBase.anchoredPosition = localPoint;

        // Manually trigger the stick's logic so it knows it just started at (0,0)
        stickComponent.OnPointerDown(eventData);
    }

    // Forward these events so the stick keeps moving
    public void OnDrag(PointerEventData eventData) => stickComponent.OnDrag(eventData);
    public void OnPointerUp(PointerEventData eventData)
    {
        stickComponent.OnPointerUp(eventData);
        joystickBase.gameObject.SetActive(false);
    }
}