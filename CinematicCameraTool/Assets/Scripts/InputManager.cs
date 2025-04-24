using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    private bool isDraggingObject = false;
    private bool isPanningCamera = false;

    public static bool LeftMouseDown => Input.GetMouseButton(0);
    public static bool LeftMousePressedThisFrame => Input.GetMouseButtonDown(0);
    public static bool LeftMouseReleasedThisFrame => Input.GetMouseButtonUp(0);
    public static bool IsOverUI => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

    public static bool IsDraggingObject => _instance?.isDraggingObject ?? false;
    public static bool IsPanningCamera => _instance?.isPanningCamera ?? false;

    public static bool CanPanCamera => !IsOverUI && !IsDraggingObject;
    public static bool CanDragObject => !IsOverUI;
    
    [SerializeField]
    private DragSceneCamera _dragSceneCamera;
    
    [SerializeField]
    private EditorCamera _editorCamera;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        Debug.Assert(_instance != null, "InputManager instance not assigned.");
    }

    private void Update() {
        _dragSceneCamera.UpdateSystem();
        _editorCamera.UpdateSystem();
    }

    public static void SetDraggingObject(bool dragging)
    {
        if (_instance != null) _instance.isDraggingObject = dragging;
    }

    public static void SetPanningCamera(bool panning)
    {
        if (_instance != null) _instance.isPanningCamera = panning;
    }
}