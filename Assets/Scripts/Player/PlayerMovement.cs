using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private const string IS_DRIFTING = "IsDrifting";

    [Header("References")]
    [SerializeField] private Rigidbody sphereRigidbody;
    [SerializeField] private Rigidbody cartRigidbody;
    [SerializeField] private LayerMask groundLayer;

    [Header("Values")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float reverseSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float modifiedDrag;
    [SerializeField] private float alignToGroundTime;

    private float moveInput;
    private float turnInput;
    private float normalDrag;

    private bool isGrounded;

    public override void OnNetworkSpawn()
    {
        InitializeRigidbodies();
    }

    private void InitializeRigidbodies()
    {
        if (IsOwner)
        {
            Debug.Log(OwnerClientId);
            sphereRigidbody.transform.parent = null;
            cartRigidbody.transform.parent = null;
            normalDrag = sphereRigidbody.drag;
        }
    }

    private void Update()
    {
        if (!IsOwner) { return; }

        if(GameManager.Instance.IsGamePlayingActive())
        {
            HandleInput();
            HandlePlayerRotation();
            HandlePlayerMovement();
            AlignToGround();
            AdjustDrag();
        }
        
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }

        if (isGrounded)
        {
            sphereRigidbody.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            sphereRigidbody.AddForce(transform.up * -20f);
        }

        cartRigidbody.MoveRotation(transform.rotation);
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void HandlePlayerRotation()
    {
        float newRotation = turnInput * turnSpeed * Time.deltaTime * moveInput;

        if (isGrounded)
        {
            transform.Rotate(0f, newRotation, 0f, Space.World);
        }
    }

    private void HandlePlayerMovement()
    {
        transform.position = sphereRigidbody.transform.position;
    }

    private void AlignToGround()
    {
        RaycastHit hit;
        float maxDistance = 2f;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, maxDistance, groundLayer);

        Quaternion rotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, alignToGroundTime * Time.deltaTime);
    }

    private void AdjustDrag()
    {
        moveInput *= moveInput > 0f ? forwardSpeed : reverseSpeed;
        sphereRigidbody.drag = isGrounded ? normalDrag : modifiedDrag;
    }
}
