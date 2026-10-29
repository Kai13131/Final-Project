using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;

    public Rigidbody rb;
    public Camera camera;

    public float MouseSensitivity = 5f;
    private float xRotation = 0f;

    public float jumpForce = 8f;
    private int jumpCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, -20f, 0);
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraMove();
    }

    void Move()
    {
        float x = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * x + transform.right * z;
        move *= speed;

        rb.linearVelocity = new Vector3(move.x,rb.linearVelocity.y,move.z);
        Jump();
    }

    void CameraMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Rotate the player horizontally
        transform.Rotate(0f, mouseX, 0f);

        // Rotate the camera vertically
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;

            if (jumpCount >= 2)
            {
                jumpForce = 8f  ;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player touched the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
