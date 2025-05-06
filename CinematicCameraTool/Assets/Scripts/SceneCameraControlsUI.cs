using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneCameraTransformUI : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;

    [Header("Height (Y Position)")]
    [SerializeField] private Slider heightSlider;
    [SerializeField] private TMP_InputField heightInput;
    
    [Header("Rotation")]
    [SerializeField] private Slider rotationXSlider;
    [SerializeField] private Slider rotationYSlider;
    [SerializeField] private Slider rotationZSlider;
    [SerializeField] private TMP_InputField rotationXInput;
    [SerializeField] private TMP_InputField rotationYInput;
    [SerializeField] private TMP_InputField rotationZInput;
    
    [SerializeField] private TMP_InputField positionXInput;
    [SerializeField] private TMP_InputField positionYInput;
    [SerializeField] private TMP_InputField positionZInput;

    private void Start()
    {
        var drag = targetCamera.GetComponent<DragSceneCamera>();
        if (drag != null)
            drag.OnPositionChanged += RefreshFromCamera;
        
        SetupListeners();
        RefreshFromCamera(); // initialize values on start
    }
    
    private void OnDestroy()
    {
        if (targetCamera != null) {
            var drag = targetCamera.GetComponent<DragSceneCamera>();
            if (drag != null)
                drag.OnPositionChanged -= RefreshFromCamera;
        }
    }

    private void SetupListeners()
    {
        // Height
        heightSlider.onValueChanged.AddListener(OnHeightSliderChanged);
        heightInput.onEndEdit.AddListener(OnHeightInputChanged);

        // Rotation sliders
        rotationXSlider.onValueChanged.AddListener(_ => OnRotationSliderChanged());
        rotationYSlider.onValueChanged.AddListener(_ => OnRotationSliderChanged());
        rotationZSlider.onValueChanged.AddListener(_ => OnRotationSliderChanged());

        // Rotation input fields
        rotationXInput.onEndEdit.AddListener(_ => OnRotationInputChanged());
        rotationYInput.onEndEdit.AddListener(_ => OnRotationInputChanged());
        rotationZInput.onEndEdit.AddListener(_ => OnRotationInputChanged());
        
        // Position input fields
        positionXInput.onEndEdit.AddListener(_ => OnPositionInputChanged());
        positionYInput.onEndEdit.AddListener(_ => OnPositionInputChanged());
        positionZInput.onEndEdit.AddListener(_ => OnPositionInputChanged());
    }

    public void RefreshFromCamera()
    {
        Vector3 pos = targetCamera.position;
        Vector3 rot = targetCamera.eulerAngles;
        
        // Position
        positionXInput.text = pos.x.ToString("F1");
        positionYInput.text = pos.y.ToString("F1");
        positionZInput.text = pos.z.ToString("F1");
        
        // Height Slider (same as Y position)
        heightSlider.SetValueWithoutNotify(pos.y);
        heightInput.text = pos.y.ToString("F2");

        // Rotation
        rotationXSlider.SetValueWithoutNotify(rot.x);
        rotationYSlider.SetValueWithoutNotify(rot.y);
        rotationZSlider.SetValueWithoutNotify(rot.z);

        rotationXInput.text = rot.x.ToString("F1");
        rotationYInput.text = rot.y.ToString("F1");
        rotationZInput.text = rot.z.ToString("F1");
    }

    // ---------- Event Handlers ----------

    private void OnHeightSliderChanged(float newY)
    {
        var pos = targetCamera.position;
        pos.y = newY;
        targetCamera.position = pos;
        heightInput.text = newY.ToString("F2");
        positionYInput.text = newY.ToString("F2");
    }

    private void OnHeightInputChanged(string value)
    {
        if (float.TryParse(value, out float newY))
        {
            var pos = targetCamera.position;
            pos.y = newY;
            targetCamera.position = pos;
            heightSlider.SetValueWithoutNotify(newY);
        }
    }

    private void OnRotationSliderChanged()
    {
        Vector3 newRotation = new Vector3(
            rotationXSlider.value,
            rotationYSlider.value,
            rotationZSlider.value
        );

        targetCamera.eulerAngles = newRotation;

        rotationXInput.text = newRotation.x.ToString("F1");
        rotationYInput.text = newRotation.y.ToString("F1");
        rotationZInput.text = newRotation.z.ToString("F1");
    }

    private void OnRotationInputChanged()
    {
        float.TryParse(rotationXInput.text, out float x);
        float.TryParse(rotationYInput.text, out float y);
        float.TryParse(rotationZInput.text, out float z);

        targetCamera.eulerAngles = new Vector3(x, y, z);

        rotationXSlider.SetValueWithoutNotify(x);
        rotationYSlider.SetValueWithoutNotify(y);
        rotationZSlider.SetValueWithoutNotify(z);
    }
    
    private void OnPositionInputChanged()
    {
        float.TryParse(positionXInput.text, out float x);
        float.TryParse(positionYInput.text, out float y);
        float.TryParse(positionZInput.text, out float z);

        targetCamera.position = new Vector3(x, y, z);
        heightInput.text = y.ToString("F2");
    }
}
