
public class MoveState : PlayerStates {
    
    public override void PassDamage() {
        characterMovement.ChangeState(new HitState(characterMovement));
    }

    public override void Movement() {
        
    }

    public override void PressAttack() {
        characterMovement.ChangeState(new AttackState(characterMovement));
    }

    public override void PressJump() {
        characterMovement.ChangeState(new JumpState(characterMovement));
    }

    public MoveState(CharacterMovement _ch) : base(_ch) {
    }
}
