using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class ControllerInputs
{
    protected Character character;

    public Myactions controls;
    public Vector2 prevMove = Vector2.zero;
    public Vector2 moveAxis = Vector2.zero;
    private float smashTurnThresh = .4f;

    public ControllerInputs(Character character){
        this.character = character;

        controls = new Myactions();

        controls.Player.Jump.performed += ctx => OnJump(ctx);
        controls.Player.Move.started += ctx => OnMove(ctx);
        controls.Player.Move.performed += ctx => OnMove(ctx);
        controls.Player.Move.canceled += ctx => OnMove(ctx);

        /*
        controls.Player.Move.started += ctx => {
            prevMove = moveAxis;
            moveAxis = ctx.ReadValue<Vector2>();
        };
        controls.Player.Move.performed += ctx => {
            prevMove = moveAxis;
            moveAxis = ctx.ReadValue<Vector2>();
        };
        controls.Player.Move.canceled += ctx => {
            prevMove = moveAxis;
            moveAxis = ctx.ReadValue<Vector2>();
        };
        */
        controls.Enable();
    }

    public void Tick(){
        prevMove = moveAxis;
    }

    public void OnJump(InputAction.CallbackContext context){
        character.OnJump();
    }

    public void OnMove(InputAction.CallbackContext context){
        //Debug.Log(context.ReadValue<Vector2>());
        moveAxis = context.ReadValue<Vector2>();
        if ((Math.Abs(moveAxis.x - prevMove.x) > smashTurnThresh) &&
             (Math.Abs(moveAxis.x) > smashTurnThresh)){
            character.SmashStick();
        }
        character.OnMove();
        
        //Debug.Log("Moving!");   
    }
}