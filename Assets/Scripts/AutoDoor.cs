using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f; // How far the door swings (use -90 to swing the other way)
    public float swingSpeed = 5f; // How fast the door opens

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private int entitiesInTrigger = 0; // Counts how many characters are near

    void Start()
    {
        // Remember our starting rotation
        closedRotation = transform.rotation;
        
        // Calculate what the rotation should be when fully opened
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        // If 1 or more people are in the zone, open. If 0, close it.
        Quaternion targetRotation = entitiesInTrigger > 0 ? openRotation : closedRotation;
        
        // Smoothly swing the door toward the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * swingSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object stepping into the zone is the Player or an enemy
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            entitiesInTrigger++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // When they leave the zone, subtract them from the count
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            entitiesInTrigger--;
            
            // A quick failsafe to ensure our math never breaks
            if (entitiesInTrigger < 0) entitiesInTrigger = 0; 
        }
    }
}
