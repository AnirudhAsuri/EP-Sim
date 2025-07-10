using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    private float groundedCheckDistance;//Simon Lee :)

    public void HandleGroundCheck()
    {
        groundedCheckDistance = (GetComponentInChildren<CapsuleCollider>().height / 2) + 0.2f;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.up, out hit, groundedCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
