using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void Start() {
        transform.parent = GameMain.instance.mainCamera.transform;
    }
}
