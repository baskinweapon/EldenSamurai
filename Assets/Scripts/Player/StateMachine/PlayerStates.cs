using UnityEngine;

public abstract class PlayerStates {
    protected PlayerMind playerMind;

    protected PlayerStates(PlayerMind _ch) {
        playerMind = _ch;
    }
    
    public abstract void PassDamage();
    public abstract void Movement();
    public abstract void PressAttack();
    
    public abstract void PressJump();
    public abstract void Fly();
}
