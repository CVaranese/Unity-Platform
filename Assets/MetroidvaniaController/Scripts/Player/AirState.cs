using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class AirState : State
{
	public Animator animator;
	public bool canDoubleJump;
	private int platformLayer = LayerMask.NameToLayer("Platform"); 
	private Collider2D lastIgnoredPlatform;

    public AirState(Character character) : base(character){
    }

    public override void Tick(){
    	var horizontalForce = character.controller.moveAxis[0] * character.airAccel;
        Vector2 forceVector = new Vector2(horizontalForce, 0);
        //character.m_Rigidbody2D.AddForce(forceVector, ForceMode2D.Impulse);
		character.m_Rigidbody2D.AddForce(forceVector);

   		Vector2 currentVelocity = character.m_Rigidbody2D.velocity;
   		currentVelocity.x = Mathf.Clamp(currentVelocity.x, -character.maxAirSpeed, character.maxAirSpeed);
   		currentVelocity.x = Mathf.Clamp(currentVelocity.x, -character.limitFallSpeed, float.MaxValue);
   		character.m_Rigidbody2D.velocity = currentVelocity;

   		if (character.controller.moveAxis.y >= 0 && lastIgnoredPlatform != null){
   			//character.coll
   			//Physics
   			Debug.Log("Unignoring!");
   			Physics2D.IgnoreCollision(lastIgnoredPlatform, character.physicsBox, false);
   		} else{
   		}
    }

    public override void OnCollisionExit2D(Collision2D collision){
    	if (character.controller.moveAxis.y > 0){
   			Debug.Log("Unignoring interrupt");
    		Physics2D.IgnoreCollision(collision.otherCollider, collision.collider, false);
    	}
    }

    public override void OnCollisionEnter2D(Collision2D collision){
    	bool groundBelow = collision.GetContact(0).normal == Vector2.up;

    	if (character.controller.moveAxis.y < 0 && collision.gameObject.layer == platformLayer){
    		Debug.Log(character.m_Rigidbody2D.velocity.y);
    		Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
   			Debug.Log("Ignoring!");
    		lastIgnoredPlatform = collision.collider;
    	} else if (character.m_Rigidbody2D.velocity.y <= 0 && groundBelow){
    		character.m_Grounded = true;
    		character.SetState(new IdleState(character));
    	}	
    }

    public override void OnStateEnter(){
    	animator = character.GetComponent<Animator>();
    	animator.SetBool("IsJumping", true);


    	canDoubleJump = true;
    }

    public override void OnStateExit(){
    	if (lastIgnoredPlatform != null){
    		Debug.Log("Unignoring on exit");
    		Physics2D.IgnoreCollision(lastIgnoredPlatform, character.physicsBox, false);
    	}
    }

    public override void OnJump()
    {
    	if (canDoubleJump){
    		canDoubleJump = false;
			character.m_Rigidbody2D.velocity = new Vector2(character.m_Rigidbody2D.velocity.x, 0);
			character.m_Rigidbody2D.AddForce(new Vector2(0f, character.m_JumpForce / 1.2f));
			animator.SetBool("IsDoubleJumping", true);
		}
    }

    public override void OnMove(){
    	// Air move
    }
}