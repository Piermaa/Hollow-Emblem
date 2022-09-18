using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool dashUnlocked = false;

	[SerializeField]PlayerSounds sounds;

	public float layerCD;
	float layerTimer;

	Animator animator;

	Rigidbody2D rb;
	Vector3 lastPosition;
	public HealthManager healthManager;
	public CharacterController2D controller;
	public float dashSpeed = 80;
	public float runSpeed = 30f;
	public float dashCoolDown = 0;
	public bool isDashing;
	public float horizontalMove = 0f;
	bool jump = false;
	bool beginingJump;
	public bool crouch = false;

	bool doubleJumped;

	private void Start()
	{

		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		healthManager = FindObjectOfType<HealthManager>();
	}

	void Update()
	{
		layerTimer -= Time.deltaTime;

		DashUpdate();
		DashCorrection();

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			
			
			animator.SetBool("Jump",true);
			//animator.SetTrigger("DoubleJump");
			StartCoroutine(BeginJump());
			if (dashUnlocked)
			{
				jump = true;
				if(!controller.CheckGround() &&!doubleJumped)
				{
					sounds.PlaySound(sounds.doubleJump);
					animator.SetBool("DoubleJumping", true);
					animator.SetTrigger("DoubleJump");
					doubleJumped = true;
					//animator.SetBool("Jump", false);
				}
				
			}
			else
			{
				if(controller.CheckGround())
				{
					sounds.PlaySound(sounds.jump);
					jump = true;
				}
			}
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}

		if (Input.GetButtonDown("Dash") && dashCoolDown<0 && dashUnlocked)
		{
			Dash();
		}


		if (horizontalMove/runSpeed != 0)
		{
			if (controller.CheckGround())
			{
				sounds.PlaySound(sounds.step);
			}
	
			animator.SetBool("Run", true);
		}
		else
		{
			animator.SetBool("Run", false);
		}

		if (controller.CheckGround() &&!beginingJump)
		{
			doubleJumped = false;
			animator.SetBool("Jump", false);
			animator.SetBool("DoubleJumping", false);
		}

		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;

	}

	IEnumerator BeginJump()
	{
		beginingJump = true;
		yield return new WaitForSeconds(0.1f);
		beginingJump = false;
	}
	//there is a exploit that if you dash against a corner in a very precise way you perform a huge jump, with this code if your Y position is very big between frames the rigidbody constrains the Y position
	void DashCorrection()
	{
		if (transform.position.y - lastPosition.y > 0.3f)
		{
			controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
		}
		else if (dashCoolDown<0.3)
		{
			
			controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		if(layerTimer<0)
		{
			gameObject.layer = 3;
		}

	}

	void FixedUpdate()
	{
		// Move our character
	

		lastPosition = transform.position;
	}
	void DashUpdate()
	{
		dashCoolDown -= Time.deltaTime;
	}
	void Dash()
	{
		sounds.PlaySound(sounds.dash);
		//print(controller.CheckGround());
		dashCoolDown = 0.3f;
		animator.SetBool("Jump", false);
		animator.SetTrigger("Dash");
		//controller.Move(horizontalMove * Time.fixedDeltaTime * dashSpeed, false, false);
		if (horizontalMove!=0)
		{
			rb.AddForce(new Vector2((horizontalMove / (Mathf.Abs(horizontalMove))), 0) * dashSpeed, ForceMode2D.Impulse);
		}
	
		layerTimer = layerCD;
		gameObject.layer = 9;
	}

}

