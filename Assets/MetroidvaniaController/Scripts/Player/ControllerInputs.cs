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
    public float smashStickThresh = .4f;

    public float movementEpsilon = .01f;
    public float movementTraction = .05f;

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
        moveAxis = context.ReadValue<Vector2>();
        // Abs change above thresh
        // new pos above dash thresh
        // 
        if ((Math.Abs(moveAxis.x - prevMove.x) > smashStickThresh) &&
            (Math.Abs(moveAxis.x) > smashStickThresh)){ 
            if ((Math.Abs(moveAxis.x) > Math.Abs(prevMove.x)) ||
                (moveAxis.x * prevMove.x < 0)){
                    character.SmashStick();
             }
        } else {
           character.OnMove();
        }
    }
}