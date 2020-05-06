using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class WalkingState : State
{
	public Animator animator;
    private IEnumerator coroutine;

    private bool dashEnded = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private float movementSmoothing = .05f;

    public WalkingState(Character character) : base(character){
    }

    public override void Tick(){
        if ((character.controller.moveAxis.x > 0) != character.m_FacingRight){
            character.Flip();
        }

        

        targetVelocity = new Vector2(character.controller.moveAxis.x * (character.m_DashForce / .8f), 0);
        targetVelocity = Vector3.SmoothDamp(character.m_Rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
        character.m_Rigidbody2D.velocity = targetVelocity;

        if (Math.Abs(targetVelocity.x) < .05){
            character.SetState(new IdleState(character));
        }
    }

    public override void OnStateEnter(){
    	animator = character.GetComponent<Animator>();
        animator.SetBool("IsRunning", true);
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
    }

    public override void SmashStick(){
        character.SetState(new DashingState(character));
    }

    public override void OnCollisionExit2D(Collision2D Collision) { 
        character.m_Grounded = false;
        character.canDoubleJump = true;
        character.SetState(new AirState(character));
    }
}