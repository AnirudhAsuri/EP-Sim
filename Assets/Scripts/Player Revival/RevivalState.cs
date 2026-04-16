using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalState : MonoBehaviour
{
    public static RevivalState Instance;

    public bool hasRevived = false;

    private void Awake()
    {
        Instance = this;
    }
}