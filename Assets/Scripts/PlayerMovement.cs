using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 movement;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevents physics rotating the player
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // movement vector
        movement = new Vector3(horizontal, 0f, vertical).normalized;

        // plays animation
        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);

    }

    void FixedUpdate()
    {
        // Move player Rigidbody
        Vector3 moveVelocity = movement * moveSpeed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        // Rotate player towards movement direction if moving
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.fixedDeltaTime));
        }
    }

    // for footstep sounds
    public void FootStep() {
        // audio (for later)
    }
}