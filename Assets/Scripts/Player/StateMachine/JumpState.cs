using UnityEngine;


public class JumpState : PlayerStates {
    private Animator animator;
    private Rigidbody2D rb;
    
    private static readonly int JumpString = Animator.StringToHash("Jump");
    
    public JumpState(PlayerMind _ch) : base(_ch) {
        animator = _ch.animator;
        rb = _ch.rb;
    }

    public override void PassDamage() {
    }

    public override void Movement() {
       
    }

    public override void PressAttack() {
        
    }

    public override void PressJump() {
        animator.SetTrigger(JumpString);
        rb.velocity += Vector2.up * playerMind.jumpMultiplier;
        playerMind.ChangeState(new FlyState(playerMind));
        playerMind.ChangeStateType(PlayerStatesType.Fly);
    }

    public override void Fly() {
        throw new System.NotImplementedException();
    }
}
