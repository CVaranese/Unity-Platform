using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{

	[SerializeField] public float m_JumpForce;							// Amount of force added when the player jumps.
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector2 velocity = Vector2.zero;
	public float limitFallSpeed = 25f; // Limit fall speed
	public BoxCollider2D physicsBox ;

	public bool canDoubleJump = true; //If player can double jump
	[SerializeField] public float m_DashForce = 1f;
	private bool canDash = true;
	private bool isDashing = false; //If player is dashing

	public float airAccel;
	public float maxAirSpeed;

	private Animator animator;

	[Header("Events")]
	[Space]

	private State currentState;

	public ControllerInputs controller;

	void OnCollisionEnter2D(Collision2D collision){
		currentState.OnCollisionEnter2D(collision);
    }

    void OnCollisionExit2D(Collision2D collision){
    	currentState.OnCollisionExit2D(collision);
    }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		controller = new ControllerInputs(this);
		physicsBox = GetComponent<BoxCollider2D>();
		SetState(new AirState(this));
	}

	private void FixedUpdate()
	{
		controller.Tick();
		currentState.Tick();
	}

	public void SetState(State state)
    {
        if (currentState != null){
            currentState.OnStateExit();
        }

        currentState = state;
        gameObject.name = "Knight - " + state.GetType().Name;

        if (currentState != null){
        	currentState.OnStateEnter();
        }
    }

    public void OnMove() {
    	currentState.OnMove();
    }

    public void OnJump()
    {
    	currentState.OnJump();
    }

    public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void SmashStick(){
		currentState.SmashStick();
	}
}
