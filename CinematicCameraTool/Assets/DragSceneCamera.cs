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
        if (InputManager.Instance.IsOverUI)
        {
            if (!isDragging && highlighter != null) highlighter.SetHighlight(false);
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (isDragging)
        {
            if (highlighter != null) highlighter.SetHighlight(true);

            if (InputManager.Instance.LeftMouseDown && dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 targetPos = hitPoint + dragOffset;
                targetPos.y = transform.position.y;
                transform.position = targetPos;
            }

            if (InputManager.Instance.LeftMouseReleasedThisFrame)
            {
                isDragging = false;
                InputManager.Instance.SetDragging(false);
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

                if (InputManager.Instance.LeftMousePressedThisFrame &&
                    InputManager.Instance.CanDragObject())
                {
                    Vector3 planeOrigin = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    dragPlane = new Plane(Vector3.up, planeOrigin);

                    if (dragPlane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        dragOffset = transform.position - hitPoint;
                        isDragging = true;
                        InputManager.Instance.SetDragging(true);
                    }
                }
            }
        }

        if (highlighter != null) highlighter.SetHighlight(hovered);
    }
}
