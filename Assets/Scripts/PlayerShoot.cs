using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Reference to the bullet prefab
    public GameObject bulletPrefab;

    // Reference to the point where bullets will appear
    public Transform firePoint;

    void Update()
    {
        // Press Space to shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // firepoint check
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint is not assigned!");
            return;
        }

        // Create a bullet at the firePoint position and rotation
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}