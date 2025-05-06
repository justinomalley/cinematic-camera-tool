using System.Collections;
using UnityEngine;
using CameraTypes;

public class CameraTimelinePlayback : MonoBehaviour
{
    [SerializeField] private Transform cinematicCamera;
    [SerializeField] private ViewToggle viewToggle;
    [SerializeField] private PlaybackUIController playbackUI;

    private CameraTimeline timeline;
    private Coroutine playbackCoroutine;

    private void Start()
    {
        timeline = CameraTimelineController.Instance.GetTimeline();
    }

    public void PlayTimeline()
    {
        if (playbackCoroutine != null) return; // Already playing
        playbackCoroutine = StartCoroutine(PlaybackRoutine());
        playbackUI.SetPlayButtonState(true);
    }

    public void StopTimeline()
    {
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }

        ResetCamera();
        playbackUI.SetPlayButtonState(false);
    }

    public void OnViewChanged()
    {
        StopTimeline(); // Stops any running playback

        if (viewToggle != null && viewToggle.editorUI.activeSelf)
        {
            MoveCameraToLatestKeyframe();
        }
        else
        {
            MoveCameraToFirstKeyframe();
        }
    }

    private IEnumerator PlaybackRoutine()
    {
        var elements = timeline.elements;
        if (elements.Count == 0)
        {
            StopTimeline();
            yield break;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] is CameraKeyframe keyframe)
            {
                cinematicCamera.position = keyframe.position;
                cinematicCamera.eulerAngles = keyframe.eulerRotation;

                if (i + 1 < elements.Count && elements[i + 1] is CameraTransition transition)
                {
                    yield return new WaitForSeconds(transition.duration);
                    i++; // Skip transition
                }
            }
        }

        StopTimeline();
    }

    private void ResetCamera()
    {
        if (viewToggle != null && viewToggle.editorUI.activeSelf)
        {
            MoveCameraToLatestKeyframe();
        }
        else
        {
            MoveCameraToFirstKeyframe();
        }
    }

    private void MoveCameraToFirstKeyframe()
    {
        var firstKeyframe = GetFirstKeyframe();
        if (firstKeyframe != null)
        {
            cinematicCamera.position = firstKeyframe.position;
            cinematicCamera.eulerAngles = firstKeyframe.eulerRotation;
        }
    }

    private void MoveCameraToLatestKeyframe()
    {
        var lastKeyframe = GetLastKeyframe();
        if (lastKeyframe != null)
        {
            cinematicCamera.position = lastKeyframe.position;
            cinematicCamera.eulerAngles = lastKeyframe.eulerRotation;
        }
    }

    private CameraKeyframe GetFirstKeyframe()
    {
        foreach (var elem in timeline.elements)
        {
            if (elem is CameraKeyframe keyframe)
                return keyframe;
        }
        return null;
    }

    private CameraKeyframe GetLastKeyframe()
    {
        for (int i = timeline.elements.Count - 1; i >= 0; i--)
        {
            if (timeline.elements[i] is CameraKeyframe keyframe)
                return keyframe;
        }
        return null;
    }
}
