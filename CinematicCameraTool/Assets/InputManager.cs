using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool LeftMouseDown { get; private set; }
    public bool LeftMousePressedThisFrame { get; private set; }
    public bool LeftMouseReleasedThisFrame { get; private set; }

    public bool IsOverUI { get; private set; }
    public bool IsDraggingObject { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        LeftMouseDown = Input.GetMouseButton(0);
        LeftMousePressedThisFrame = Input.GetMouseButtonDown(0);
        LeftMouseReleasedThisFrame = Input.GetMouseButtonUp(0);
        IsOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    public void SetDragging(bool isDragging)
    {
        IsDraggingObject = isDragging;
    }

    public bool CanPanCamera()
    {
        return !IsDraggingObject && !IsOverUI;
    }

    public bool CanDragObject()
    {
        return !IsOverUI;
    }
}