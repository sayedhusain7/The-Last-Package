using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 12f;
    public float mouseSensitivity = 200f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float xRotation = 0f;
    private float yRotation = 180f;
    private bool isGrounded;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate() {
        float moveH = Input.GetAxisRaw("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.forward * moveV) + (transform.right * moveH);
        moveDirection = moveDirection.normalized * speed;

        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }
}