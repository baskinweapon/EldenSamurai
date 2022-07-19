using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour {
    private void Start() {
        Player.instance.bodyTransform.position = transform.position;
    }
}
