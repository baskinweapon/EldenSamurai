using UnityEngine;

public class Portal : MonoBehaviour {
    public SpriteRenderer keySprite;
    public InteractionAsset interactionAsset;

    private void Start() {
        keySprite.sprite = interactionAsset.keyVisual;
        keySprite.gameObject.SetActive(false);
        InputSystem.OnInteraction += InteractionInput;
    }
    
    private void InteractionInput() {
        if (!isStay) return;
        SceneController.instance.LoadScene(2);
    }


    private bool isStay;
    private void OnTriggerEnter2D(Collider2D col) {
        if (!col.CompareTag("Player")) return;
        keySprite.gameObject.SetActive(true);
        isStay = true;
    }


    private void OnTriggerExit2D(Collider2D other) {
        keySprite.gameObject.SetActive(false);
        isStay = false;
    }

    private void OnDisable() {
        InputSystem.OnInteraction -= InteractionInput;
    }
}
