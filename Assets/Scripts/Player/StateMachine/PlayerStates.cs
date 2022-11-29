using UnityEngine;

public enum PlayerStatesType {
    Idle,
    Run,
    Attack,
    Hit,
    Jump,
}

public abstract class PlayerStates {
    protected CharacterMovement characterMovement;

    protected PlayerStates(CharacterMovement _ch) {
        characterMovement = _ch;
    }
    
    public abstract void PassDamage();
    public abstract void Movement();
    public abstract void PressAttack();
    
    public abstract void PressJump();
}
