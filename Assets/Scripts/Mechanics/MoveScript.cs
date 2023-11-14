using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 6f;

    public ParticleSystem splash1;
    public ParticleSystem splash2;

    public bool onFloor;

    private void Start()
    {
        splash2.Stop();
        onFloor = true;
    }


    private void FixedUpdate()
    {
        HandleJoystickMovement();
    }

    void HandleJoystickMovement()
    {
        float horizontal = joystick.Vertical;
        float vertical = joystick.Horizontal;
        Vector3 direction = new Vector3(-horizontal, 0f, vertical).normalized;

        if (controller.enabled)
        {
            controller.SimpleMove(direction * moveSpeed * Time.deltaTime);

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                float targetAngle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                if (onFloor)
                {
                    splash2.Play();
                }
            }
            else
            {
                animator.SetBool("Running", false);
                splash2.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            splash1.Play();
            splash2.Play();
            onFloor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            splash1.Stop();
            splash2.Stop();
            onFloor = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }
}