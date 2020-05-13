using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class IdleState : State
{
	public Animator animator;
    private int _currentGround;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;

    public IdleState(Character character) : base(character){
    }

    public override void Tick(){
        if (Math.Abs(character.controller.moveAxis.x) > 0){
            character.SetState(new WalkingState(character));
        } else {
            targetVelocity = new Vector2(character.controller.moveAxis.x * (character.m_DashForce / .8f), 0);
            targetVelocity = Vector3.SmoothDamp(character.m_Rigidbody2D.velocity, targetVelocity, ref velocity, character.controller.movementTraction);
            character.m_Rigidbody2D.velocity = targetVelocity;
        }
    }

    public override void OnStateEnter(){
    	character.animator.Play("Iddle");
    }

    public override void OnStateExit(){
    }

    public override void OnCollisionStay2D(Collision2D collision){
        _currentGround = collision.collider.gameObject.layer;
    }

    public override void OnJump()
    {
        GroundedJump();
    }

    public override void OnMove(){
        if (character.controller.moveAxis.y < 0 && _currentGround == platformLayer){ // check platform vs not platform
            character.SetState(new AirState(character));
        }
    }

    public override void SmashStick(){
        character.SetState(new DashingState(character));
    }
}