﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private bool isRunning = false;

	public bool isOutside = true;
	public float walkSpeed = 0.6f;
    public float runSpeed = 3.5f;
    public float turnSmoothing = 15f;

	Animator anim;
    

    Rigidbody rb;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
    }

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");

		MoveCharacter (moveHorizontal, moveVertical);
    }

    void Update()
    {
        if (Input.GetButton("Run") || isOutside == true)
        {
            isRunning = true;
			anim.SetBool("Is Running", true);
        } else {
            isRunning = false;
			anim.SetBool("Is Running", false);
        }
    }

    void MoveCharacter(float h, float v) {
		if (h != 0f || v != 0f) {
			Vector3 movement = new Vector3 (h, 0f, v);
			Rotating (h, v);

            // Normalise the movement vector and make it proportional to the speed per second.

            if (isRunning)
            {
                movement = movement.normalized * runSpeed * Time.deltaTime;
            }
            else {
                movement = movement.normalized * walkSpeed * Time.deltaTime;
            }
            
			
			// Move the player to it's current position plus the movement.
			rb.MovePosition (transform.position + movement);
		}
	}
	
	void Rotating(float horizontal, float vertical) {
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rb.MoveRotation(newRotation);
	}
}
