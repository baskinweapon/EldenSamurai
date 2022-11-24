using UnityEngine;


public class JumpState : PlayerStates {
    public JumpState(CharacterMovement _ch) : base(_ch) {
    }

    public override void PassDamage() {
        characterMovement.ChangeState(new HitState(characterMovement));
    }

    public override void Movement() {
        characterMovement.ChangeState(new MoveState(characterMovement));
    }

    public override void PressAttack() {
        characterMovement.ChangeState(new AttackState(characterMovement));
    }

    public override void PressJump() {
        
    }
}
