using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    private PlayerAnimations playerAnimations;

    public InputActionReference leftAttack;
    public InputActionReference rightAttack;

    private void Start()
    {
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
    }

    private void OnEnable()
    {
        leftAttack.action.started += LeftAttack;

        rightAttack.action.started += RightAttack;
    }

    private void OnDisable()
    {
        leftAttack.action.started -= LeftAttack;

        rightAttack.action.started -= RightAttack;
    }

    private void RightAttack(InputAction.CallbackContext obj)
    {
        playerAnimations.ActivateRightAttack();
    }

    private void LeftAttack(InputAction.CallbackContext obj)
    {
        playerAnimations.ActivateLeftAttack();
    }
}