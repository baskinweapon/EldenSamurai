
public class IdleState : PlayerStates {
    
    public override void PassDamage() {
        playerMind.ChangeState(new HitState(playerMind));
    }

    public override void Movement() {
        playerMind.ChangeState(new MoveState(playerMind));
    }

    public override void PressAttack() {
        playerMind.ChangeState(new AttackState(playerMind));
    }

    public override void PressJump() {
        playerMind.ChangeState(new JumpState(playerMind));
    }

    public override void Fly() {
        throw new System.NotImplementedException();
    }

    public IdleState(PlayerMind _ch) : base(_ch) {
    }
}
