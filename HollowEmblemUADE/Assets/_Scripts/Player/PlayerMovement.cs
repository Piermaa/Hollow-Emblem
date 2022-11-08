using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	[Header("Classes")]
	[SerializeField]PlayerSounds sounds;
	Animator animator;
	Rigidbody2D rb;
	PlayerCombat combat;
	Vector3 lastPosition;
	public HealthManager healthManager;
	public CharacterController2D controller;


	[Header("Floats")]
	public float dashSpeed = 80;
	public float runSpeed = 30f;
	public float dashCoolDown = 0;
	public float maxDashCoolDown = 0.5f;
	public float horizontalMove = 0f;
	public float layerCD;
	float layerTimer;


	[Header("Bools")]
	bool doubleJumped;
	bool jump = false;
	bool beginingJump;
	public bool crouch = false;
	public bool isDashing;
	public bool dashUnlocked = false;

	public Image dashUI;
	public GameObject dashUIGameObject;

	private void Start()
	{
		combat = GetComponent<PlayerCombat>();
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		healthManager = FindObjectOfType<HealthManager>();
	}

	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		layerTimer -= Time.deltaTime;
		
		DashUpdate();
		DashCorrection();
		UIDash();

		Run();
		Jump();

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
	}

	IEnumerator BeginJump()
	{
		beginingJump = true;
		yield return new WaitForSeconds(0.1f);
		beginingJump = false;
	}
	//there is a exploit that if you dash against a corner in a very precise way you perform a huge jump, with this code if your Y position is very big between frames the rigidbody constrains the Y position

     /// <summary>
     /// Freezes the player if it moves very much on Y
     /// </summary>
	void DashCorrection()
	{
		if (layerTimer < 0)
		{
			gameObject.layer = 3;
		}
		else
		{
			rb.velocity = new Vector2(rb.velocity.x, 0); // evita que el jugador se mueva en Y al dashear
		}
	}

	void FixedUpdate()
	{
		// Move our character
		
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		if (combat.aiming)
		{
			rb.velocity = new Vector2(0, 0); //evita que se mueva el jugador al apuntar
		}
			jump = false;
		lastPosition = transform.position;
	}

    /// <summary>
    /// Detects the jump button and checks if player is grounded. Executes the jump, double jump and their respective animations.
    /// </summary>
	void Jump()
	{
		if (Input.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") >= 0)
		{
			animator.SetBool("Jump", true);
			//animator.SetTrigger("DoubleJump");
			StartCoroutine(BeginJump());
			if (dashUnlocked)
			{
				jump = true;

				if (!controller.CheckGround() && !doubleJumped)
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
				if (controller.CheckGround())
				{
					sounds.PlaySound(sounds.jump);
					jump = true;
				}
			}
		}

		if (controller.CheckGround() && !beginingJump)
		{
			doubleJumped = false;
			animator.SetBool("Jump", false);
			animator.SetBool("DoubleJumping", false);
		}
	}

    /// <summary>
    /// Sets player movement and running animation
    /// </summary>
	void Run()
	{
		if (horizontalMove / runSpeed != 0)
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
	}
	void DashUpdate()
	{
		dashCoolDown -= Time.deltaTime;
	}

    /// <summary>
    /// Performs a horizontal impulse, it depends of where the player is facing and not where he i moving
    /// </summary>
    void Dash()
    {
        sounds.PlaySound(sounds.dash);
        dashCoolDown = maxDashCoolDown;
        animator.SetBool("Jump", false);
        animator.SetTrigger("Dash");
        //controller.Move(horizontalMove * Time.fixedDeltaTime * dashSpeed, false, false);

        Vector3 theScale = transform.localScale;
        
        rb.AddForce(new Vector2(-(theScale.x / (Mathf.Abs(theScale.x))), 0) * dashSpeed, ForceMode2D.Impulse);
		rb.velocity = new Vector2(rb.velocity.x, 0);
		layerTimer = layerCD;
		gameObject.layer = 9;
	}

	void UIDash()
    {
		dashUI.fillAmount = dashCoolDown / maxDashCoolDown;

		if (dashUnlocked)
        {
			dashUIGameObject.SetActive(true);
        }

        else
        {
			dashUIGameObject.SetActive(false);
        }
    }
    
}