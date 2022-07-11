using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake() {
        Player.instance.transform.position = transform.position;
    }
}
