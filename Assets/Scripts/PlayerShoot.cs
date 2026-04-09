using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public TPSAimController aimController;
    public Camera aimCamera;
    public float maxAimDistance = 500f;
    public LayerMask aimMask = ~0;

    Transform cachedRoot;

    void Awake()
    {
        cachedRoot = transform.root;
    }

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

        if (aimCamera == null) aimCamera = Camera.main;
        if (aimCamera == null) return;

        Vector3 aimPoint = GetAimPoint();

        Vector3 directionToAim = (aimPoint - firePoint.position);
        if (directionToAim.sqrMagnitude < 0.0001f) return;

        if (Vector3.Dot(directionToAim, aimCamera.transform.forward) < 0f)
        {
            aimPoint = firePoint.position + aimCamera.transform.forward * maxAimDistance;
            directionToAim = (aimPoint - firePoint.position);
            if (directionToAim.sqrMagnitude < 0.0001f) return;
        }

        Quaternion rotation = Quaternion.LookRotation(directionToAim.normalized, Vector3.up);

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, rotation);
        BulletMovement bullet = bulletObject.GetComponent<BulletMovement>();
        if (bullet != null)
        {
            // bullet.SetDirection(directionToAim.normalized);
        }
    }

    Vector3 GetAimPoint()
    {
        if (aimController != null && aimController.aimTarget != null)
        {
            Vector3 aimPoint = aimController.aimTarget.position;
            if (Vector3.Dot(aimPoint - aimCamera.transform.position, aimCamera.transform.forward) > 0f)
            {
                return aimPoint;
            }
        }

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.RaycastAll(ray, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore);
        if (hits != null && hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            for (int i = 0; i < hits.Length; i++)
            {
                Collider hitCollider = hits[i].collider;
                if (hitCollider == null) continue;

                Transform hitRoot = hitCollider.transform.root;
                if (cachedRoot != null && hitRoot == cachedRoot) continue;

                return hits[i].point;
            }
        }

        return ray.origin + ray.direction * maxAimDistance;
    }
}
