using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMovement : MonoBehaviour
{
	ShootScript shootScript;
	public float shootCounter;

	Vector3 lastPosition;
	Vector2 movement;

	public CharacterController2D controller;
	public float dashSpeed = 80;
	public float runSpeed = 1f;
	public float dashCoolDown = 0;
	public bool isDashing;
	public float horizontalMove = 0f;
	public float verticalMove = 0f;
	bool jump = false;
	public bool crouch = false;

	private void Start()
	{
		shootScript = GetComponent<ShootScript>();
		//unSpeed = speedBackup;
	}
	// Update is called once per frame
	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

		movement = new Vector2(horizontalMove,verticalMove);
		movement.Normalize();


	}
	
	void FixedUpdate()
	{
		// Move our character
		transform.Translate(movement *5* Time.deltaTime);

		//controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;

		lastPosition = transform.position;
	}


}
