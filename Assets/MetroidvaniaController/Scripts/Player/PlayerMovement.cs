using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	private Rigidbody2D m_Rigidbody2D;
	BoxCollider2D m_Collider2D;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;

	bool dashAxis = false;
	
	// Update is called once per frame
	public void OnMove(InputAction.CallbackContext context)
    {
    	var value = context.ReadValue<Vector2>();
        horizontalMove = value[0] * runSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    	if (context.started){
    		Debug.Log("Jumping!");
    		jump = true;
    	}
    }

    void Awake(){
    	m_Rigidbody2D = GetComponent<Rigidbody2D>();
    	m_Collider2D = GetComponent<BoxCollider2D>();
    }

	void Update () {
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
	}

	public void OnFall()
	{
		Debug.Log("Fall!");
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		Debug.Log("Land!");
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}
