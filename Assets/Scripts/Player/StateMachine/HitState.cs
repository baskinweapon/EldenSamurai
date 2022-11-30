using UnityEngine;

public class HitState : PlayerStates {
    private Collider2D col;
    private Rigidbody2D rb;
    private Animator animator;
    
    public HitState(PlayerMind _ch, Collider2D col = null) : base(_ch) {
        this.col = col;
        rb = _ch.rb;
        animator = _ch.animator;
    }
    
    public override void PassDamage() {
        var velocity = -col.attachedRigidbody.velocity;
        
        animator.SetTrigger("Damaged");
        rb.AddForce(Vector2.Reflect(velocity,Vector2.up) * 1000f);
    }

    public override void Movement() {
        
    }

    public override void PressAttack() {
    }

    public override void PressJump() {
        
    }

    public override void Fly() {
    }

    
}
