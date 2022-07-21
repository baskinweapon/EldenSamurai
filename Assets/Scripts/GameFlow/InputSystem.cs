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
        playerInput.Player.Menu.performed += MenuPressed;
    }

    private void FixedUpdate() {
        moveVector = playerInput.Player.Move.ReadValue<Vector2>();
    }
    
#region CallbackFunctions

    private void Jump(InputAction.CallbackContext ctx) {
        OnJump?.Invoke();
    }

    private void InteractionInput(InputAction.CallbackContext ctx) {
        OnInteraction?.Invoke();
    }

    private bool isMenuOpen;
    public void MenuPressed(InputAction.CallbackContext ctx = default) {
        isMenuOpen = !isMenuOpen;
        PauseManager.instance.Pause(isMenuOpen);
    }

#endregion
    
    
    public Vector2 GetMoveVector() {
        return moveVector;
    }
        
    public bool IsJumping() {
        return playerInput.Player.Jump.IsPressed();
    }
    
    

    private void OnDestroy() {
        playerInput.Player.Jump.performed -= Jump;
        playerInput.Player.Interaction.performed -= InteractionInput;
        playerInput.Player.Menu.performed -= MenuPressed;
    }
}
