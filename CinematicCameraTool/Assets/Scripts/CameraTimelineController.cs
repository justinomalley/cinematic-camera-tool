using System.Collections.Generic;
using UnityEngine;
using CameraTypes;

public class CameraTimelineController : MonoBehaviour
{
    public static CameraTimelineController Instance { get; private set; }

    [SerializeField] private Transform cameraToRecord;

    private CameraTimeline timeline = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddKeyframe()
    {
        // Create keyframe from current camera transform
        var keyframe = new CameraKeyframe
        {
            position = cameraToRecord.position,
            eulerRotation = cameraToRecord.eulerAngles,
        };

        CameraTransition transition = null;

        // Insert transition before the new keyframe if there’s already one present
        if (timeline.elements.Count > 0 && timeline.elements[^1] is CameraKeyframe) {
            transition = new CameraTransition {
                type = TransitionType.Cut,
                duration = 1f,
            };
            timeline.elements.Add(transition);
        }

        timeline.elements.Add(keyframe);
        DebugPrintKeyframe(keyframe);
        TimelineUIController.Instance.AddTimelineVisuals(keyframe, transition);
    }
    
    public void DebugPrintKeyframe(CameraKeyframe keyframe)
    {
        if (keyframe == null)
        {
            Debug.LogWarning("Tried to print a null keyframe.");
            return;
        }

        Debug.Log($"Keyframe: Pos({keyframe.position.x:F2}, {keyframe.position.y:F2}, {keyframe.position.z:F2}) | " +
                  $"Rot({keyframe.eulerRotation.x:F1}, {keyframe.eulerRotation.y:F1}, {keyframe.eulerRotation.z:F1})");
    }
    
    public CameraTimeline GetTimeline()
    {
        return timeline;
    }

    public CameraKeyframe GetFirstKeyframe()
    {
        foreach (var elem in timeline.elements)
        {
            if (elem is CameraKeyframe keyframe)
                return keyframe;
        }
        return null;
    }
}