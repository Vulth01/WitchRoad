using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TrainManager : MonoBehaviour
{
    public delegate void DoorOpenAction();
    public event DoorOpenAction OnDoorOpen;

    public bool interactionComplete;

    private void Awake()
    {
        interactionComplete = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TriggerDoorOpen();
        }
    }

    private void TriggerDoorOpen()
    {
        OnDoorOpen?.Invoke();
    }
}