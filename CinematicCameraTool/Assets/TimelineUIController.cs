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
            Instantiate(keyframePrefab, contentContainer);
        }

        // Optional: Scroll to end
        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(1, 0);
    }
}