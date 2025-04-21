using UnityEngine;

/// <summary>
/// Allows camera panning across the XZ plane using right-click drag,
/// simulating an editor-style viewport. Y position stays fixed.
/// </summary>
public class EditorStyleCameraPan : MonoBehaviour
{
    [Header("Panning Settings")]
    [SerializeField] private float dragSpeed = 0.1f;
    [SerializeField] private float dragSmoothing = 0.1f;

    [Header("Movement Bounds")]
    [SerializeField] private int minX = -20;
    [SerializeField] private int maxX = 20;
    [SerializeField] private int minZ = -20;
    [SerializeField] private int maxZ = 20;

    private Vector3 dragOrigin;
    private Vector3 targetPosition;
    private bool isDragging;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right-click to start drag
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-diff.x, 0, -diff.y) * dragSpeed * Time.deltaTime;

            Vector3 right = transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up); // XZ plane forward

            targetPosition += right * move.x + forward * move.z;
            dragOrigin = Input.mousePosition;
        }

        // Clamp to bounds before applying
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, dragSmoothing);
    }
}