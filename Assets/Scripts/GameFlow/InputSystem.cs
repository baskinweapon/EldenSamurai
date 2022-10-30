using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputSystem : Singleton<InputSystem> {
    private EldenSamurai playerInput;

    public static Action OnJump;
    public static Action OnInteraction;

    
    //ability input
    public static Action OnFirstAbility;
    public static Action OnSecondAbility;
    public static Action OnThirdAbility;
    public static Action OnFourthAbility;
    
    protected override void Awake() {
        base.Awake();
        
        playerInput = new EldenSamurai();
        playerInput.Enable();
        
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Interaction.performed += InteractionInput;
        playerInput.Player.Menu.performed += MenuPressed;
        
        playerInput.Player.FirstAbility.performed += FirstAbilityPress;
        playerInput.Player.SecondAbility.performed += SecondAbilityPress;
        playerInput.Player.ThirdAbility.performed += ThirdAbilityPress;
        playerInput.Player.FourthAbility.performed += FourthAbilityPress;
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

    private void FirstAbilityPress(InputAction.CallbackContext ctx = default) {
       OnFirstAbility?.Invoke();
    }
    
    private void SecondAbilityPress(InputAction.CallbackContext ctx = default) {
        OnSecondAbility?.Invoke();
    }
    
    private void ThirdAbilityPress(InputAction.CallbackContext ctx = default) {
        OnThirdAbility?.Invoke();
    }
    
    private void FourthAbilityPress(InputAction.CallbackContext ctx = default) {
        OnThirdAbility?.Invoke();
    }
    
#endregion

    public Vector2 GetMoveVector() {
        return playerInput.Player.Move.ReadValue<Vector2>();;
    }

    public Vector2 GetMousePosition() {
        return playerInput.UI.Point.ReadValue<Vector2>();
    }
        
    public bool IsJumping() {
        return playerInput.Player.Jump.IsPressed();
    }
    
    

    private void OnDestroy() {
        playerInput.Player.Jump.performed -= Jump;
        playerInput.Player.Interaction.performed -= InteractionInput;
        playerInput.Player.Menu.performed -= MenuPressed;
        
        playerInput.Player.FirstAbility.performed -= FirstAbilityPress;
        playerInput.Player.SecondAbility.performed -= SecondAbilityPress;
        playerInput.Player.ThirdAbility.performed -= ThirdAbilityPress;
        playerInput.Player.FourthAbility.performed -= FourthAbilityPress;
    }
}
