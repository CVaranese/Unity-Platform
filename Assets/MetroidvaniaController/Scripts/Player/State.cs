using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public abstract class State
{
    protected Character character;

    protected static int platformLayer = LayerMask.NameToLayer("Platform"); 
	protected Vector2 jumpForce;

    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnJump() {}
    public virtual void OnMove() {}
    public virtual void SmashStick() {}
    public virtual void OnCollisionEnter2D(Collision2D collision) {}
    public virtual void OnCollisionExit2D(Collision2D Collision) { }
    public virtual void OnCollisionStay2D(Collision2D Collision) { }

    protected void GroundedJump(){
        Debug.Log("Jumping");

		jumpForce = new Vector2(character.controller.moveAxis.x, 1.0f);
		jumpForce.Normalize();
		jumpForce *= character.m_JumpConstant;


		Debug.Log(jumpForce);
		//character.m_Rigidbody2D.AddForce(new Vector2(character.controller.moveAxis.x * character.m_JumpForce * character.m_JumpConstant, character.m_JumpForce));
		//character.m_Rigidbody2D.velocity = new Vector2(character.controller.moveAxis.x * character.m_JumpForce * character.m_JumpConstant, );
		character.m_Rigidbody2D.velocity = jumpForce + new Vector2(.7f*character.m_Rigidbody2D.velocity.x, 0);


		if (character.m_Grounded){
			character.canDoubleJump = true;
		}
        character.m_Grounded = false;
        character.SetState(new AirState(character));
    }

    public State(Character character)
    {
        this.character = character;
    }
}