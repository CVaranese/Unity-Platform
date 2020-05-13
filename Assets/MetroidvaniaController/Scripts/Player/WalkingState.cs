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
    private float movementSmoothing = .025f;

    public WalkingState(Character character) : base(character){
    }

    public override void Tick(){
        targetVelocity = new Vector2(character.controller.moveAxis.x * (character.m_DashForce * .8f), 0);
        targetVelocity = Vector3.SmoothDamp(character.m_Rigidbody2D.velocity, targetVelocity, ref velocity, character.controller.movementTraction);
        character.m_Rigidbody2D.velocity = targetVelocity;
        if (Mathf.Abs(targetVelocity.x) < character.controller.movementEpsilon){
            character.SetState(new IdleState(character));
        } else if ((targetVelocity.x > 0) != character.m_FacingRight){
            character.Flip();
        }   
    }

    public override void OnStateEnter(){
        character.animator.Play("Run");
        targetVelocity = Vector3.zero;
    }

    public override void OnStateExit(){
    }

    public override void OnJump()
    {
        GroundedJump();
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