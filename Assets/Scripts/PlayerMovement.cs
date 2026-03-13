using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Transform cam;

    private Animator animator;
    private Rigidbody rb;
    //private Vector3 movement;
    private Vector3 moveDirection;
    private float targetAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevents physics rotating the player
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("Speed", input.magnitude);

        if (input.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // movement vector
        //movement = new Vector3(horizontal, 0f, vertical).normalized;

        // plays animation
        //float speed = movement.magnitude;
        //animator.SetFloat("Speed", speed);

    }

    void FixedUpdate()
    {
        // Move player Rigidbody
        if (moveDirection.magnitude >= 0.1f)
        {
            //Vector3 moveVelocity = movement * moveSpeed;
            Vector3 moveVelocity = moveDirection.normalized * moveSpeed;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

            // Rotate player towards movement direction if moving
            // if (movement != Vector3.zero)
            //{
            //    Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            Quaternion toRotation = Quaternion.Euler(0f, targetAngle, 0f);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.fixedDeltaTime));
            //}
        }
    }

    // for footstep sounds
    public void FootStep()
    {
        // audio (for later)
    }
}