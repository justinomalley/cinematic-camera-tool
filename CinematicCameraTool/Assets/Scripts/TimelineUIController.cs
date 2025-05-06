using CameraTypes;
using UnityEngine;
using UnityEngine.UI;

public class TimelineUIController : MonoBehaviour
{
    public static TimelineUIController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private RectTransform contentContainer;
    [SerializeField] private GameObject keyframePrefab;
    [SerializeField] private GameObject transitionPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddTimelineVisuals(CameraKeyframe keyframe, CameraTransition transition)
    {
        if (transition != null && transitionPrefab != null)
        {
            var transitionController = Instantiate(transitionPrefab, contentContainer)
                .GetComponent<TransitionUIController>();
            transitionController.Init(transition);
        }

        if (keyframePrefab != null)
        {
            var keyframeObj = Instantiate(keyframePrefab, contentContainer);
            var uiController = keyframeObj.GetComponent<KeyframeUIController>();
            uiController.Init(keyframe);
        }

        // Optional: Scroll to end
        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(1, 0);
    }

    public void DeleteKeyframe(CameraKeyframe keyframeToDelete, GameObject keyframeUIObject)
    {
        var timeline = CameraTimelineController.Instance.GetTimeline();

        int index = timeline.elements.IndexOf(keyframeToDelete);
        if (index == -1)
        {
            Debug.LogWarning("Tried to delete a keyframe not found in the timeline.");
            return;
        }

        // --- Updated deletion logic ---

        // Case 1: First keyframe → remove the transition *after* it (if it exists)
        if (index == 0 && timeline.elements.Count > 1 && timeline.elements[1] is CameraTransition)
        {
            timeline.elements.RemoveAt(1);
        }
        // Case 2: Any other keyframe → remove the transition *before* it
        else if (index > 0 && timeline.elements[index - 1] is CameraTransition)
        {
            timeline.elements.RemoveAt(index - 1);
            index--; // Adjust index because we removed one before
        }

        // Remove the keyframe itself
        timeline.elements.RemoveAt(index);

        // Remove the keyframe UI object
        Destroy(keyframeUIObject);

        // Best practice: just rebuild the UI for now
        RefreshTimelineUI();
    }

    public void RefreshTimelineUI()
    {
        // Clear all UI children
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Rebuild the UI from the current timeline data
        var elements = CameraTimelineController.Instance.GetTimeline().elements;

        foreach (var element in elements)
        {
            if (element is CameraTransition transition)
            {
                var transitionController = Instantiate(transitionPrefab, contentContainer)
                    .GetComponent<TransitionUIController>();
                transitionController.Init(transition);
            }
            else if (element is CameraKeyframe keyframe)
            {
                var keyframeObj = Instantiate(keyframePrefab, contentContainer);
                var uiController = keyframeObj.GetComponent<KeyframeUIController>();
                uiController.Init(keyframe);
            }
        }
    }
}
