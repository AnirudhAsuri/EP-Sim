using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    private float groundedCheckDistance; //Simon Lee :)

    private string sandyLayer = "Sandy Floor";
    private string grassyLayer = "Grassy Floor";
    private string icyLayer = "Icy Floor";

    public int grassLayerInt, sandLayerInt, iceLayerInt;

    public enum FloorType
    {
        GrassyFloor,
        SandyFloor,
        IcyFloor,
        Air
    };

    public FloorType currentFloorType;

    private void Awake()
    {
        grassLayerInt = LayerMask.NameToLayer(grassyLayer);
        sandLayerInt = LayerMask.NameToLayer(sandyLayer);
        iceLayerInt = LayerMask.NameToLayer(icyLayer);
    }

    public void HandleGroundCheck()
    {
        float skinWidth = 0.1f;
        float rayDistance = 0.2f;

        Vector3 rayStart = transform.position + (Vector3.up * skinWidth);
        RaycastHit hit;

        // Shoot the ray
        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        HandleFloorType(hit);
    }

    private void HandleFloorType(RaycastHit hit)
    {
        if(isGrounded)
        {
            if (hit.transform.gameObject.layer == grassLayerInt)
            {
                currentFloorType = FloorType.GrassyFloor;
            }

            else if (hit.transform.gameObject.layer == sandLayerInt)
            {
                currentFloorType = FloorType.SandyFloor;
            }

            else if (hit.transform.gameObject.layer == iceLayerInt)
            {
                currentFloorType = FloorType.IcyFloor;
            }
        }

        else
        {
            currentFloorType = FloorType.Air;
        }
    }
}
