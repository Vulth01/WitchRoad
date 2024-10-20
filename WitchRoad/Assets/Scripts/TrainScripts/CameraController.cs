using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    private Vector3 velocity = Vector3.zero;

    enum CameraState
    {
        CARRIAGE,
        CARDGAME
    }
    
    private CameraState state = CameraState.CARRIAGE;
    private void Update()
    {
        if(state is not CameraState.CARRIAGE) return;
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.transform.TransformPoint(new Vector3(0, 20, -8));
    
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
    }
}
