using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerDummy : MonoBehaviour
{
    private PlayerAnimations playerAnimations;

    private void Start()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
    }

    public void HandleMainMenuDummyAttack()
    {
        playerAnimations.RightAttack();
    }
}