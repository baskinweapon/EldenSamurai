using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : Singleton<InputSystem> {
    public EldenSamurai playerInput;

    public static Action OnJump;
    public static Action OnInteraction;

    private Vector2 moveVector;
    protected override void Awake() {
        base.Awake();
        
        playerInput = new EldenSamurai();
        playerInput.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Interaction.performed += InteractionInput;
    }

    private void FixedUpdate() {
        moveVector = playerInput.Player.Move.ReadValue<Vector2>();
    }
    
    private void Jump(InputAction.CallbackContext ctx) {
        OnJump?.Invoke();
    }

    private void InteractionInput(InputAction.CallbackContext ctx) {
        OnInteraction?.Invoke();
    }
    
    public Vector2 GetMoveVector() {
        return moveVector;
    }
        
    public bool IsJumping() {
        return playerInput.Player.Jump.IsPressed();
    }

    private void OnDestroy() {
        playerInput.Player.Jump.performed -= Jump;
        playerInput.Player.Interaction.performed -= InteractionInput;
    }
}
