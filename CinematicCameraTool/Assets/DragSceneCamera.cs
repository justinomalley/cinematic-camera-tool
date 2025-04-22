using UnityEngine;
using UnityEngine.EventSystems;

public class DragSceneCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Plane dragPlane;
    private Vector3 dragOffset;
    private bool isDragging = false;

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    // Lock the Y and create a drag plane
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

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 targetPos = hitPoint + dragOffset;
                targetPos.y = transform.position.y; // Lock Y
                transform.position = targetPos;
            }
        }
    }
}