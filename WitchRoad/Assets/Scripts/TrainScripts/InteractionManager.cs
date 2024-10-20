using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class InteractionManager : MonoBehaviour
{
    GameObject playerCam;
    private InputAction interact;
    private PlayerInput input;
    private bool isInteracting = false;
    private bool isDoorOpen = false;


    public delegate void NextRoomEvent();
    public event NextRoomEvent OnNextRoom;


    private void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        input = GetComponent<PlayerInput>();
        interact = input.actions["interact"];
        TrainManager trainManager = FindObjectOfType<TrainManager>();
        if (trainManager != null) trainManager.OnDoorOpen += DoDoorOpen;
    }


    private void DoDoorOpen()
    {
        isDoorOpen = true;
    }


    private void OnTriggerStay(Collider collider)
    {
        if (!interact.IsPressed()) { return; }


        if (!isInteracting && collider.CompareTag("Table"))
        {
            Debug.Log("TABLE!!!");
            isInteracting = true;
            DoTable();
        }

    }

    private void DoTable()
    {
        isDoorOpen = true;
        //SceneManager.LoadScene(2);
    }

    private void DoDoor()
    {
        OnNextRoom?.Invoke();
    }


}
