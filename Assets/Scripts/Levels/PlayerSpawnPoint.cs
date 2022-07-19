using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour {
    private void Start() {
        Player.instance.visualTransform.position = transform.position;
    }
}
