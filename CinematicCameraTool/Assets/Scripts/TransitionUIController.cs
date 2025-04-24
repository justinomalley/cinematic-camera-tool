using UnityEngine;
using TMPro;
using CameraTypes;

public class TransitionUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField durationInput;
    [SerializeField] private TMP_Dropdown typeDropdown;

    private CameraTransition linkedTransition;

    public void Init(CameraTransition transition)
    {
        linkedTransition = transition;

        // Sync initial UI state
        durationInput.text = linkedTransition.duration.ToString("0.##");
        typeDropdown.value = (int)linkedTransition.type;

        // Register listeners
        durationInput.onEndEdit.AddListener(UpdateDuration);
        typeDropdown.onValueChanged.AddListener(UpdateType);
    }

    private void UpdateDuration(string value)
    {
        if (float.TryParse(value, out float seconds))
        {
            linkedTransition.duration = Mathf.Max(0f, seconds);
        }
    }

    private void UpdateType(int index)
    {
        linkedTransition.type = (TransitionType)index;
    }
}