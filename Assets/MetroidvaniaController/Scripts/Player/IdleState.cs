using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class IdleState : State
{
	public Animator animator;

    public IdleState(Character character) : base(character){
    }

    public override void Tick(){
    }

    public override void OnStateEnter(){
    	animator = character.GetComponent<Animator>();
    	animator.SetBool("IsJumping", false);
        animator.SetBool("IsRunning", false);
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
        character.SetState(new WalkingState(character));
    }

    public override void SmashStick(){
        character.SetState(new DashingState(character));
    }
}