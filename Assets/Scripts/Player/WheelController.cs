using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WheelController : NetworkBehaviour
{
    private const string IS_GOING_LEFT = "IsGoingLeft";
    private const string IS_GOING_RIGHT = "IsGoingRight";

    [Header("References")]
    [SerializeField] private GameObject[] wheelsToRotate;
    [SerializeField] private TrailRenderer[] wheelTrails;

    [Header("Values")]
    [SerializeField] private float rotationSpeed;

    private float horizontalAxis;
    private float verticalAxis;
    private Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        if (!IsOwner) { return; }

        HandleInput();
        HandleWheelRotation();
        SetWheelAnimations();
        SetWheelTrails();
    }

    private void HandleInput()
    {
        verticalAxis = Input.GetAxisRaw("Vertical");
        horizontalAxis = Input.GetAxisRaw("Horizontal");
    }

    private void HandleWheelRotation()
    {
        foreach(GameObject wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(verticalAxis * rotationSpeed * Time.deltaTime, 0f, 0f, Space.Self);
        }
    }

    private void SetWheelAnimations()
    {
        if(horizontalAxis > 0f)
        {
            //TURNING RIGHT
            animator.SetBool(IS_GOING_LEFT, false);
            animator.SetBool(IS_GOING_RIGHT, true);
        }
        else if(horizontalAxis < 0f)
        {
            //TURNING LEFT
            animator.SetBool(IS_GOING_LEFT, true);
            animator.SetBool(IS_GOING_RIGHT, false);
        }
        else
        {
            //GOING STRAIGHT
            animator.SetBool(IS_GOING_LEFT, false);
            animator.SetBool(IS_GOING_RIGHT, false);
        }
    }

    private void SetWheelTrails()
    {
        if(horizontalAxis != 0f)
        {
            foreach(TrailRenderer wheelTrail in wheelTrails)
            {
                wheelTrail.emitting = true;
            }
            
        }
        else
        {
            foreach(TrailRenderer wheelTrail in wheelTrails)
            {
                wheelTrail.emitting = false;
            }
        }
    }
}
