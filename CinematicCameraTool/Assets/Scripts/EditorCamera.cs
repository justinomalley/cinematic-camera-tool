using UnityEngine;

/// <summary>
/// Allows camera panning across the XZ plane using left-click drag,
/// and zooming in/out along the camera's forward vector using the scroll wheel.
/// </summary>
public class EditorCamera : MonoBehaviour
{
    [Header("Panning Settings")]
    [SerializeField] private float dragSpeed = 0.1f;
    [SerializeField] private float dragSmoothing = 0.1f;

    [Header("Movement Bounds")]
    [SerializeField] private int minX = -20;
    [SerializeField] private int maxX = 20;
    [SerializeField] private int minZ = -20;
    [SerializeField] private int maxZ = 20;

    [Header("Zoom Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoomOffset = -10f;
    [SerializeField] private float maxZoomOffset = 10f;

    private Vector3 dragOrigin;
    private Vector3 targetPosition;
    private bool isDragging;

    private Vector3 initialCameraLocalPosition;

    void Start()
    {
        targetPosition = transform.position;

        if (cameraTransform == null)
        {
            Debug.LogWarning("Camera Transform not assigned. Disabling zoom.");
        }
        else
        {
            initialCameraLocalPosition = cameraTransform.localPosition;
        }
    }

    public void UpdateSystem()
    {
        HandlePanning();
        HandleZoom();
    }

    private void HandlePanning()
    {
        if (!InputManager.CanPanCamera)
            return;

        if (InputManager.LeftMousePressedThisFrame && InputManager.CanPanCamera)
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
            InputManager.SetPanningCamera(true);
        }

        if (InputManager.LeftMouseReleasedThisFrame)
        {
            isDragging = false;
            InputManager.SetPanningCamera(false);
        }

        if (isDragging && InputManager.LeftMouseDown)
        {
            Vector3 diff = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-diff.x, 0, -diff.y) * dragSpeed * Time.deltaTime;

            Vector3 right = transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up); // Flattened forward

            targetPosition += right * move.x + forward * move.z;
            dragOrigin = Input.mousePosition;
        }

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, dragSmoothing);
    }

    private void HandleZoom()
    {
        if (cameraTransform == null)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f))
            return;

        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        Vector3 offset = cameraTransform.localPosition - initialCameraLocalPosition;
        float projectedOffset = Vector3.Dot(offset, flatForward);

        float newProjectedOffset = projectedOffset + scroll * zoomSpeed;

        if (newProjectedOffset < minZoomOffset || newProjectedOffset > maxZoomOffset)
            return;

        cameraTransform.localPosition += flatForward * scroll * zoomSpeed;
    }
}
