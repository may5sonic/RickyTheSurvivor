using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal,0f,vertical);

        transform.Translate(movement * moveSpeed * Time.deltaTime);

        float speed = movement.magnitude;

        animator.SetFloat("Speed", speed);

    }

    public void FootStep() {
        
    }
}