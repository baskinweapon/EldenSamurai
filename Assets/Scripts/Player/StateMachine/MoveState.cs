
using UnityEngine;

public class MoveState : PlayerStates {
    private Rigidbody2D rb;
    private Transform vision;
    private Animator animator;
    
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    
    public MoveState(PlayerMind _ch) : base(_ch) {
        rb = _ch.rb;
        vision = _ch.vision;
        animator = _ch.animator;
    }
    
    public override void PassDamage() {
        playerMind.ChangeState(new HitState(playerMind));
    }

    public override void Movement() {
        var move = InputSystem.instance.GetMoveVector();

        animator.SetFloat(AnimState, Mathf.InverseLerp(0, 1, Mathf.Abs(move.x)));
        if (move.x == 0f) return;

        if (move.x > 0) vision.localScale = new Vector3(Mathf.Abs(vision.localScale.x), vision.localScale.y, vision.localScale.z);
        else if (move.x < 0) vision.localScale = new Vector3(-Mathf.Abs(vision.localScale.x), vision.localScale.y, vision.localScale.z);;
        
        Vector2 velocity = new Vector2(move.x * playerMind.playerSpeed * Time.deltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    public override void PressAttack() {
        playerMind.ChangeState(new AttackState(playerMind));
    }

    public override void PressJump() {
        playerMind.ChangeState(new JumpState(playerMind));
        playerMind.currentState.PressJump();
    }

    public override void Fly() {
    }
}
