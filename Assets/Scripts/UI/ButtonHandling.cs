using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonHandling : MonoBehaviour
{
    public Vector2 mousePosition;

    private void Update()
    {

    }

    public bool HandleMouseHoweringOverButton()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        {
            mousePosition = Input.mousePosition;
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach(RaycastResult raycastResult in raycastResults)
        {
            if(raycastResult.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }
}