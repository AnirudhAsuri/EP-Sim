using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSpot : MonoBehaviour
{
    private string playerTag = "Player";
    [SerializeField] private Tutorial tutorial;
    public bool isReached= false;

    private void Awake()
    {
        tutorial = FindAnyObjectByType<Tutorial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (!isReached)
                tutorial.IncrementCurrentTextPointer();
            isReached = true;
            Destroy(gameObject);
        }
    }
}