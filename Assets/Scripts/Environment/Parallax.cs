
using UnityEngine;

public class Parallax : MonoBehaviour {
    private float lenght, startPos;
    
    public Transform cam;

    public float parallaxEffect;

    private void Start() {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = CameraController.instance.mainCamera.transform;
    }

    private void FixedUpdate() {
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
