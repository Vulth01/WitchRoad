using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Windows;

public class InteractionManager : MonoBehaviour
{
    GameObject playerCam;
    private InputAction interact;
    private PlayerInput input;
    private bool isInteracting = false;


    private void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        input = GetComponent<PlayerInput>();
        interact = input.actions["interact"];
    }


    private void FixedUpdate()
    {


    }


    private void OnTriggerStay(Collider collider)
    {
        if (!interact.IsPressed()) { return; }
        
        
        if (!isInteracting && collider.CompareTag("Interactable"))
        {
            isInteracting = true;
            DoTable();
        }

    }

    private void DoTable()
    {
        Debug.Log("DO TABLE");

    }


}
