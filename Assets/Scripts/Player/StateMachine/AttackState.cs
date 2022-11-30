using UnityEngine;

public class AttackState : PlayerStates {
    private Animator animator;
    
    public AttackState(PlayerMind _ch) : base(_ch) {
        animator = _ch.animator;
    }

    public override void PassDamage() {
        playerMind.ChangeState(new HitState(playerMind));
    }

    public override void Movement() {
        playerMind.ChangeState(new MoveState(playerMind));
    }

    public override void PressAttack() {
        animator.SetTrigger("Attack");
    }

    public override void PressJump() {
    }

    public override void Fly() {
        
    }
}
