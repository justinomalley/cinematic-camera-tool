using UnityEngine;
using UnityEngine.UI;

public class PlaybackUIController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button stopButton;

    [SerializeField] private CameraTimelinePlayback playbackController;

    private bool isPlaying = false;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        stopButton.onClick.AddListener(OnStopPressed);
        UpdateButtonVisuals(false);
    }

    private void OnPlayPressed()
    {
        if (!isPlaying)
        {
            playbackController.PlayTimeline();
            SetPlayButtonState(true);
        }
    }

    private void OnStopPressed()
    {
        playbackController.StopTimeline();
        SetPlayButtonState(false);
    }

    // Called by CameraTimelinePlayback when playback stops or is interrupted
    public void SetPlayButtonState(bool playing)
    {
        isPlaying = playing;
        UpdateButtonVisuals(playing);
    }

    private void UpdateButtonVisuals(bool playing)
    {
        // Simple example: make play button non-interactable while playing
        playButton.interactable = !playing;
        stopButton.interactable = playing;

        // Optional: swap button colors or icons if you want to give more feedback
    }
}