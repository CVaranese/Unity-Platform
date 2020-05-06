using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class ControllerStickViewer : State
{
    public ControllerStickViewer(Character character) : base(character){
    }

    public override void Tick() {
    	Vector2 angleVect = character.controller.moveAxis;
		angleVect.Normalize();
		float stickAngle = Mathf.Atan2(-angleVect.x, angleVect.y) * Mathf.Rad2Deg;
		Quaternion target = Quaternion.Euler(0, 0, stickAngle);
		character.transform.rotation = target;
    }

    /*
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnJump() {}
    public virtual void OnMove() {}
    public virtual void SmashTurn() {}
	*/

	public override void OnJump(){
	}

	public override void OnMove(){
		
		//character.transform.Rotate(0, 0, 360 * character.controller.moveAxis.x);
	}

	public override void SmashStick(){
		Debug.Log("Smash!");
		character.transform.position += new Vector3(character.controller.moveAxis.x, 0, 0);
	}

	public override void OnStateEnter() { }	


}