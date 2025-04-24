using System.Collections.Generic;
using UnityEngine;

namespace CameraTypes
{
    public enum TransitionType
    {
        Cut,
        Lerp,
        Spline
    }

    [System.Serializable]
    public abstract class TimelineElement
    {
        public float timestamp;
    }

    [System.Serializable]
    public class CameraKeyframe : TimelineElement
    {
        public Vector3 position;
        public Vector3 eulerRotation;
    }

    [System.Serializable]
    public class CameraTransition : TimelineElement
    {
        public TransitionType type = TransitionType.Lerp;
        public float duration = 1f;
    }

    [System.Serializable]
    public class CameraTimeline
    {
        public List<TimelineElement> elements = new();
    }
}