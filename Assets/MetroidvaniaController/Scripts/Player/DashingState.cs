using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class DashingState : State
{
	public Animator animator;
    private IEnumerator coroutine;

    private bool dashEnded = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private float movementSmoothing = .05f;

    public DashingState(Character character) : base(character){
    }

    public override void Tick(){
        if (dashEnded){
            character.SetState(new WalkingState(character));
        } else {
            targetVelocity = new Vector2(character.controller.moveAxis.x * character.m_DashForce, 0);
            targetVelocity = Vector3.SmoothDamp(character.m_Rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
            character.m_Rigidbody2D.velocity = targetVelocity;
        }
    }

    public override void OnStateEnter(){
    	animator = character.GetComponent<Animator>();
        animator.SetBool("IsDashing", true);
        int moveDirection = (int) (character.controller.moveAxis.x / Math.Abs(character.controller.moveAxis.x));
        if ((moveDirection > 0) != character.m_FacingRight){
            character.Flip();
        }
        character.m_Rigidbody2D.velocity = new Vector2((moveDirection * character.m_DashForce),
                                                        character.m_Rigidbody2D.velocity.y);
        coroutine = EndDashState();
        character.StartCoroutine(coroutine);
    }

    public override void OnStateExit(){
    }

    public override void OnJump()
    {
        character.m_Rigidbody2D.AddForce(new Vector2(0f, character.m_JumpForce));
        character.m_Grounded = false;
        character.canDoubleJump = true;
        character.SetState(new AirState(character));
    }

    public override void OnMove(){
        //character.SetState(new WalkingState(character));
    }

    public override void SmashStick(){
        character.SetState(new DashingState(character));
    }

    private IEnumerator EndDashState(){
        yield return new WaitForSeconds(.4f);
        dashEnded = true;
    }

    public override void OnCollisionExit2D(Collision2D Collision) { 
        character.m_Grounded = false;
        character.canDoubleJump = true;
        character.SetState(new AirState(character));
    }
}