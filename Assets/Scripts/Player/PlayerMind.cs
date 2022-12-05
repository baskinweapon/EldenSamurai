using System;
using UnityEngine;

public enum PlayerStatesType {
	Movement,
	Attack,
	Hit,
	Fly,
}

public class PlayerMind : MonoBehaviour {
	public PlayerStatesType stateType;
	
	[Header("Animation")]
	public Animator animator;
	public Transform vision;
	
	[Header("Physics")]
	[SerializeField] private CapsuleCollider2D col;
	public Rigidbody2D rb;
	public float playerSpeed = 10f;

	[Header("Jump Settings")]
	public float lowJumpMultiplier = 1f;
	public float gravityMultiplier = 1f;
	public float jumpMultiplier = 1f;
	public float jumpMovementSpeed;

	public float attackCastingTime = 0.3f;
	public float hitTime = 0.3f;
	
	[SerializeField] private GameObject attackColider;

	public Action<PlayerStatesType> OnChangeStateType;
	public Action<bool> OnChangeIsGround;

	public PlayerStates currentState;
	public void ChangeState(PlayerStates _state) {
		currentState = _state;
	}

	public void ChangeStateType(PlayerStatesType _states) {
		stateType = _states;
		
		OnChangeStateType?.Invoke(stateType);
	}

	private void Start() {
		InputSystem.OnJump += Jump;
		InputSystem.OnFirstAbility += Attack;

		currentState = new IdleState(this);
		
		ChangeStateType(PlayerStatesType.Movement);
	}
	
	private void Jump() {
		if (!isGrounded) return;
		
		currentState.PressJump();
	}
	
	

	
	private bool isCasting;
	private void Attack() {
		if (isCasting || isDamaging) return;
		currentState = new AttackState(this);
		ChangeStateType(PlayerStatesType.Attack);
		currentState.PressAttack();
		
		isCasting = true;
		attackColider.SetActive(true);
		Invoke(nameof(EndCasting), attackCastingTime);
	}

	private bool isDamaging;
	public void PassDamage(float value, Collider2D _col) {
		if (isDamaging) return;
		currentState = new HitState(this, _col);
		ChangeStateType(PlayerStatesType.Hit);
		currentState.PassDamage();

		isDamaging = true;
		Invoke(nameof(EndHitTime), hitTime);
	}
	
	private static readonly int AirSpeedY = Animator.StringToHash("VelocityY");
	
	private void FixedUpdate() {
		isGrounded = IsGrounded();
		
		Debug.Log("State type = " + stateType + " Current State = " + currentState);

	
		
		// switch (rb.velocity.y) {
		// 	case <= 0:
		// 		rb.velocity += Physics2D.gravity.y  * gravityMultiplier * Vector2.up;
		// 		break;
		// 	case > 0 when !InputSystem.instance.IsJumping():
		// 		rb.velocity += Physics2D.gravity.y * lowJumpMultiplier * Vector2.up;
		// 		break;
		// }
		
		// Anim
		var lerp = Mathf.InverseLerp(0, -1, rb.velocity.y);
		animator.SetFloat(AirSpeedY, lerp);
		
		switch (stateType) {
			case PlayerStatesType.Movement:
				currentState.Movement();
				break;
			case PlayerStatesType.Attack:
				rb.velocity = new Vector2(0f, 0f);
				break;
			case PlayerStatesType.Hit:
				break;
			case PlayerStatesType.Fly:
				currentState.Fly();
				break;
		}
		
		// if (rb.velocity.y <= 0) {
		// 	rb.velocity += Physics2D.gravity.y  * gravityMultiplier * Vector2.up;
		// } else {
		// 	rb.velocity += Physics2D.gravity.y * lowJumpMultiplier * Vector2.up;	
		// }
	}

	private void EndHitTime() {
		isDamaging = false;
		
		currentState = new IdleState(this);
		ChangeStateType(PlayerStatesType.Movement);
	}
	
	public void EndCasting() {
		attackColider.SetActive(false);
		isCasting = false;

		ChangeStateType(PlayerStatesType.Movement);
	}
	
	private static readonly int Grounded = Animator.StringToHash("IsGround");
	public bool isGrounded;
	private const float extraHeight = .05f;
	private bool prevIsGround;
	private bool IsGrounded() {
		var bound = col.bounds;
		RaycastHit2D hit = Physics2D.Raycast(bound.center, Vector2.down, bound.extents.y + extraHeight, LayerMask.GetMask("Obstacle"));
		var isGround = hit.collider != null;

		// Debug
		var rayColor = isGround ? Color.green : Color.red;
		Debug.DrawRay(bound.center, Vector2.down * (bound.extents.y + extraHeight), rayColor);
        
		animator.SetBool(Grounded, isGround);
		if (prevIsGround != isGround) {
			OnChangeIsGround?.Invoke(isGround);
		}
		prevIsGround = isGround;
		return isGround;
	}

	private void OnDestroy() {
		InputSystem.OnJump -= Jump;
		InputSystem.OnFirstAbility -= Attack;
	}
}
 