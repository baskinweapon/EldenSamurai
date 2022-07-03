using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public float movementSpeed = 1f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate() {
        var currentPos = rb.position;
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var inputVector = new Vector2(horizontalInput, verticalInput);
        
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        var movement = inputVector * movementSpeed;
        var newPos = currentPos + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
