using UnityEngine;
using UnityEngine.EventSystems;

public class DragSceneCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Plane dragPlane;
    private Vector3 dragOffset;
    private bool isDragging;

    private HoverHighlighter highlighter;

    private void Start()
    {
        highlighter = GetComponent<HoverHighlighter>();
    }

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (!isDragging && highlighter != null) highlighter.SetHighlight(false);
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (isDragging)
        {
            // Keep the camera highlighted during drag
            if (highlighter != null) highlighter.SetHighlight(true);

            if (Input.GetMouseButton(0) && dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 targetPos = hitPoint + dragOffset;
                targetPos.y = transform.position.y;
                transform.position = targetPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            return;
        }

        // Not dragging: handle hover and drag initiation
        bool hovered = false;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                hovered = true;

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 planeOrigin = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    dragPlane = new Plane(Vector3.up, planeOrigin);

                    if (dragPlane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        dragOffset = transform.position - hitPoint;
                        isDragging = true;
                    }
                }
            }
        }

        if (highlighter != null) highlighter.SetHighlight(hovered);
    }
}
