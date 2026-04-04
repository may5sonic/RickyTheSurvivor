using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TPSAimController : MonoBehaviour
{
    public enum ReticleMode
    {
        CenterScreen,
        FreeCursor
    }

    public Camera aimCamera;
    public Transform aimTarget;

    public Canvas canvas;
    public RectTransform crosshair;

    public ReticleMode reticleMode = ReticleMode.CenterScreen;

    public bool lockCursor = true;
    public float sensitivity = 1f;
    public Vector2 start01 = new Vector2(0.5f, 0.5f);
    public Vector2 clampMin01 = new Vector2(0.1f, 0.1f);
    public Vector2 clampMax01 = new Vector2(0.9f, 0.9f);

    public float aimDistance = 25f;
    public LayerMask aimMask = ~0;
    public float aimTargetSmoothTime = 0.04f;

    Vector2 cursor01;
    RectTransform canvasRect;
    Vector3 aimTargetVelocity;

    public Vector2 Cursor01 => cursor01;
    public Vector2 CursorPixels => new Vector2(cursor01.x * Screen.width, cursor01.y * Screen.height);

    void Awake()
    {
        if (aimCamera == null) aimCamera = Camera.main;

        if (canvas == null) canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("HUD", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = short.MaxValue;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        canvasRect = canvas.transform as RectTransform;

        if (crosshair == null)
        {
            Transform existing = canvas.transform.Find("Crosshair");
            if (existing != null) crosshair = existing as RectTransform;
        }

        if (crosshair == null)
        {
            GameObject crosshairObject = new GameObject("Crosshair", typeof(RectTransform), typeof(Image));
            crosshairObject.transform.SetParent(canvas.transform, false);
            crosshair = crosshairObject.GetComponent<RectTransform>();

            crosshair.anchorMin = new Vector2(0.5f, 0.5f);
            crosshair.anchorMax = new Vector2(0.5f, 0.5f);
            crosshair.pivot = new Vector2(0.5f, 0.5f);
            crosshair.sizeDelta = new Vector2(14f, 14f);

            Image image = crosshairObject.GetComponent<Image>();
            image.raycastTarget = false;
            image.color = Color.white;
            image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f));
        }

        if (aimTarget == null)
        {
            GameObject targetObject = new GameObject("AimTarget");
            aimTarget = targetObject.transform;
        }

        cursor01 = new Vector2(Mathf.Clamp01(start01.x), Mathf.Clamp01(start01.y));
        cursor01.x = Mathf.Clamp(cursor01.x, clampMin01.x, clampMax01.x);
        cursor01.y = Mathf.Clamp(cursor01.y, clampMin01.y, clampMax01.y);

        ApplyCursorState();
        UpdateCrosshairUI();
        UpdateAimTarget();
    }

    void OnEnable()
    {
        ApplyCursorState();
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        ApplyCursorState();
        UpdateCursor();
        UpdateCrosshairUI();
        UpdateAimTarget();
    }

    void ApplyCursorState()
    {
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateCursor()
    {
        if (reticleMode == ReticleMode.CenterScreen)
        {
            cursor01 = new Vector2(0.5f, 0.5f);
            return;
        }

        Vector2 delta = GetMouseDelta();
        float safeWidth = Mathf.Max(1f, Screen.width);
        float safeHeight = Mathf.Max(1f, Screen.height);

        cursor01.x = Mathf.Clamp(cursor01.x + (delta.x / safeWidth) * sensitivity, clampMin01.x, clampMax01.x);
        cursor01.y = Mathf.Clamp(cursor01.y + (delta.y / safeHeight) * sensitivity, clampMin01.y, clampMax01.y);
    }

    void UpdateCrosshairUI()
    {
        if (canvasRect == null || crosshair == null) return;

        if (reticleMode == ReticleMode.CenterScreen)
        {
            crosshair.anchoredPosition = Vector2.zero;
            return;
        }

        Vector2 screenPoint = CursorPixels;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out Vector2 localPoint))
        {
            crosshair.anchoredPosition = localPoint;
        }
    }

    void UpdateAimTarget()
    {
        if (aimCamera == null || aimTarget == null) return;

        Vector2 screenPoint = reticleMode == ReticleMode.CenterScreen ? new Vector2(Screen.width * 0.5f, Screen.height * 0.5f) : CursorPixels;
        Ray ray = aimCamera.ScreenPointToRay(screenPoint);

        Vector3 targetPos = ray.origin + ray.direction * aimDistance;
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, aimMask, QueryTriggerInteraction.Ignore))
        {
            targetPos = hit.point;
        }

        if (aimTargetSmoothTime <= 0f)
        {
            aimTarget.position = targetPos;
            return;
        }

        aimTarget.position = Vector3.SmoothDamp(aimTarget.position, targetPos, ref aimTargetVelocity, aimTargetSmoothTime);
    }

    static Vector2 GetMouseDelta()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null) return Mouse.current.delta.ReadValue();
#endif
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }
}
