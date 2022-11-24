using UnityEngine;


public class AttackState : PlayerStates {
    public AttackState(CharacterMovement _ch) : base(_ch) {
    }

    public override void PassDamage() {
        characterMovement.ChangeState(new HitState(characterMovement));
    }

    public override void Movement() {
        characterMovement.ChangeState(new MoveState(characterMovement));
    }

    public override void PressAttack() {
        
    }

    public override void PressJump() {
        characterMovement.ChangeState(new JumpState(characterMovement));
    }
}
