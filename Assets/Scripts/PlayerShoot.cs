using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public TPSAimController aimController;
    public Camera aimCamera;
    public float maxAimDistance = 500f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint is not assigned!");
            return;
        }

        Quaternion rotation = firePoint.rotation;

        Transform target = aimController != null ? aimController.aimTarget : null;
        if (target != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            if (direction.sqrMagnitude > 0.0001f)
            {
                rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }
        else
        {
            if (aimCamera == null) aimCamera = Camera.main;

            if (aimCamera != null)
            {
                Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Vector3 direction = ray.direction;
                if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, ~0, QueryTriggerInteraction.Ignore))
                {
                    direction = (hit.point - firePoint.position).normalized;
                }

                if (direction.sqrMagnitude > 0.0001f)
                {
                    rotation = Quaternion.LookRotation(direction, Vector3.up);
                }
            }
        }

        Instantiate(bulletPrefab, firePoint.position, rotation);
    }
}
