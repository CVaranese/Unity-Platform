using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class AirState : State
{
	public Animator animator;
	
	private Collider2D lastIgnoredPlatform;

	private bool _colliding;

    public AirState(Character character) : base(character){
    }

    public override void Tick(){
    	var horizontalForce = character.controller.moveAxis[0] * character.airAccel;
        Vector2 forceVector = new Vector2(horizontalForce, 0);
        //character.m_Rigidbody2D.AddForce(forceVector, ForceMode2D.Impulse);
		character.m_Rigidbody2D.AddForce(forceVector);

   		Vector2 currentVelocity = character.m_Rigidbody2D.velocity;
   		currentVelocity.x = Mathf.Clamp(currentVelocity.x, -character.maxAirSpeed, character.maxAirSpeed);
   		currentVelocity.y = Mathf.Clamp(currentVelocity.y, -character.limitFallSpeed, float.MaxValue);
   		character.m_Rigidbody2D.velocity = currentVelocity;

		if (character.controller.moveAxis.y <= -character.controller.smashStickThresh){
			Physics2D.IgnoreLayerCollision(platformLayer, character.layer);
		} else if (! _colliding) {
			Physics2D.IgnoreLayerCollision(character.layer, platformLayer, false);
		}

		/*
   		if (character.controller.moveAxis.y >= 0 && lastIgnoredPlatform != null){
   			//character.coll
   			//Physics
   			Physics2D.IgnoreCollision(lastIgnoredPlatform, character.physicsBox, false);
   		} else{
   		}*/
    }

    public override void OnCollisionExit2D(Collision2D collision){
		_colliding = false;
    }

    public override void OnCollisionEnter2D(Collision2D collision){
    	bool groundBelow = collision.GetContact(0).normal == Vector2.up;
		_colliding = true;

		if (character.m_Rigidbody2D.velocity.y <= 0 && groundBelow){
    		character.m_Grounded = true;
    		character.SetState(new IdleState(character));
    	}	
    }

    public override void OnStateEnter(){
    	character.animator.Play("Jump");
    }

    public override void OnStateExit(){
    }

    public override void OnJump()
    {
    	if (character.canDoubleJump){
			GroundedJump();
		}
		character.canDoubleJump = false;
    }

    public override void OnMove(){
    	// Air move
    }
}