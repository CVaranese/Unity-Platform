using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public abstract class State
{
    protected Character character;

    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnJump() {}
    public virtual void OnMove() {}
    public virtual void SmashStick() {}
    public virtual void OnCollisionEnter2D(Collision2D collision) {}
    public virtual void OnCollisionExit2D(Collision2D Collision) { }

    public State(Character character)
    {
        this.character = character;
    }
}