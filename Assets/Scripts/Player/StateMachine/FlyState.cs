using System.Collections;
using UnityEngine;

public class FlyState : PlayerStates {
	private Rigidbody2D rb;
	private Transform vision;
	private Animator animator;
	
	private static readonly int AirSpeedY = Animator.StringToHash("VelocityY");
	
	public FlyState(PlayerMind _ch) : base(_ch) {
		rb = _ch.rb;
		vision = _ch.vision;
		animator = _ch.animator;
	}

	public override void PassDamage() {
		
	}

	public override void Movement() {
		var move = InputSystem.instance.GetMoveVector();
        
		if (move.x > 0) vision.localScale = new Vector3(Mathf.Abs(vision.localScale.x), vision.localScale.y, vision.localScale.z);
		else if (move.x < 0) vision.localScale = new Vector3(-Mathf.Abs(vision.localScale.x), vision.localScale.y, vision.localScale.z);;
		
		Vector2 velocity = new Vector2(move.x * playerMind.playerSpeed * Time.deltaTime, rb.velocity.y);
		rb.velocity = velocity;
	}
	
	public override void PressAttack() {
		
	}

	public override void PressJump() {
		
	}

	public override void Fly() {
		Movement();
		switch (rb.velocity.y) {
			case <= 0:
				rb.velocity += Physics.gravity.y * Time.deltaTime * Vector2.up;
				break;
			case > 0 when !InputSystem.instance.IsJumping():
				rb.velocity += Physics.gravity.y * playerMind.lowJumpMultiplier * Time.deltaTime * Vector2.up;
				break;
		}


		// Anim
		var lerp = Mathf.InverseLerp(0, -1, rb.velocity.y);
		animator.SetFloat(AirSpeedY, lerp);

		if (playerMind.isGrounded && rb.velocity.y < 0) {
			playerMind.ChangeState(new IdleState(playerMind));
			playerMind.ChangeStateType(PlayerStatesType.Movement);
		}
	}
}
