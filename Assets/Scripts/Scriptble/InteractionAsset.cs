using UnityEngine;

[CreateAssetMenu(fileName = "FILENAME", menuName = "Game/InteractionAsset", order = 0)]
public class InteractionAsset : ScriptableObject {
    public GameObject visualInteraction;
    public Sprite keyVisual;
}
