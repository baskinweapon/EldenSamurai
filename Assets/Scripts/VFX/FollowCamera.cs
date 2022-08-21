using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void Start() {
        transform.parent = CameraController.instance.mainCamera.transform;
    }
}
