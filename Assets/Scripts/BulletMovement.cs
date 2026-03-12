using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    // Speed of the bullet
    public float bulletSpeed = 10f;

    // Lifetime so bullets do not stay forever in the scene
    public float bulletLifetime = 3f;

    void Start()
    {
        // Destroy the bullet automatically after a few seconds
        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        // Move the bullet forward in the direction it is facing
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // If the bullet touches an enemy, destroy the enemy
        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}