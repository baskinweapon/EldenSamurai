
public class IdleState : PlayerStates {
    
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
        characterMovement.ChangeState(new JumpState(characterMovement));
    }

    public IdleState(CharacterMovement _ch) : base(_ch) {
    }
}
