using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interaction: MonoBehaviour {
    public SpriteRenderer keySprite;
    
    private InteractionAsset interactionAsset;
    protected bool isStay;

    private void OnEnable() {
        InputSystem.OnInteraction += InteractionInput;
    }

    protected virtual void Start() {
        interactionAsset = GameMain.instance.interactionAsset;
        keySprite.sprite = interactionAsset.keyVisual;
        keySprite.gameObject.SetActive(false);
    }
    
    protected void OnTriggerEnter2D(Collider2D col) {
        if (!col.CompareTag("Player")) return;
        keySprite.gameObject.SetActive(true);
        isStay = true;
    }

    protected void OnTriggerExit2D(Collider2D other) {
        keySprite.gameObject.SetActive(false);
        isStay = false;
    }
    
    private void OnDisable() {
        InputSystem.OnInteraction -= InteractionInput;
    }

    private void InteractionInput() {
        if (!isStay) return;
        InteractionProcess();
    }

    protected abstract void InteractionProcess();
}
