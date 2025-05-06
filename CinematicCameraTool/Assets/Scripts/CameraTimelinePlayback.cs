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
            if (elements[i] is CameraKeyframe startKeyframe)
            {
                // Snap to the start keyframe
                cinematicCamera.position = startKeyframe.position;
                cinematicCamera.eulerAngles = startKeyframe.eulerRotation;

                // If a transition and next keyframe exist
                if (i + 2 < elements.Count && elements[i + 1] is CameraTransition transition && elements[i + 2] is CameraKeyframe endKeyframe)
                {
                    if (transition.type == TransitionType.Cut)
                    {
                        yield return new WaitForSeconds(transition.duration);
                        cinematicCamera.position = endKeyframe.position;
                        cinematicCamera.eulerAngles = endKeyframe.eulerRotation;
                    }
                    else
                    {
                        float elapsed = 0f;
                        float duration = Mathf.Max(transition.duration, 0.01f); // Prevent zero duration

                        // --- Position control points ---
                        Vector3 previousPosition = startKeyframe.position; // p0
                        Vector3 startPosition = startKeyframe.position;    // p1
                        Vector3 endPosition = endKeyframe.position;        // p2
                        Vector3 nextNextPosition = endKeyframe.position;   // p3

                        if (transition.type == TransitionType.Spline)
                        {
                            if (i - 2 >= 0 && elements[i - 2] is CameraKeyframe previousKeyframe)
                                previousPosition = previousKeyframe.position;

                            if (i + 4 < elements.Count && elements[i + 4] is CameraKeyframe nextNextKeyframe)
                                nextNextPosition = nextNextKeyframe.position;
                        }

                        // --- Rotation start and end ---
                        Quaternion startRotation = Quaternion.Euler(startKeyframe.eulerRotation);
                        Quaternion endRotation = Quaternion.Euler(endKeyframe.eulerRotation);

                        // --- Interpolation loop ---
                        while (elapsed < duration)
                        {
                            float linearT = elapsed / duration;
                            // SmoothStep eases in and out
                            float t = Mathf.SmoothStep(0f, 1f, linearT);

                            // --- Position interpolation ---
                            Vector3 newPosition;
                            if (transition.type == TransitionType.Lerp || previousPosition == startPosition || endPosition == nextNextPosition)
                            {
                                newPosition = Vector3.Lerp(startPosition, endPosition, t);
                            }
                            else
                            {
                                newPosition = CatmullRom(previousPosition, startPosition, endPosition, nextNextPosition, t);
                            }

                            // --- Rotation interpolation ---
                            Quaternion newRotation = Quaternion.Slerp(startRotation, endRotation, t);

                            cinematicCamera.position = newPosition;
                            cinematicCamera.rotation = newRotation;

                            elapsed += Time.deltaTime;
                            yield return null;
                        }

                        // Snap to exact final position/rotation
                        cinematicCamera.position = endPosition;
                        cinematicCamera.eulerAngles = endKeyframe.eulerRotation;
                    }

                    i++; // Skip the transition
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

    // ---- Catmull-Rom spline helper ----
    private static Vector3 CatmullRom(Vector3 prev, Vector3 start, Vector3 end, Vector3 nextNext, float t)
    {
        return 0.5f * (
            2f * start +
            (-prev + end) * t +
            (2f * prev - 5f * start + 4f * end - nextNext) * (t * t) +
            (-prev + 3f * start - 3f * end + nextNext) * (t * t * t)
        );
    }
}
