using CameraTypes;
using UnityEngine;
using UnityEngine.UI;

public class KeyframeUIController : MonoBehaviour
{
    private CameraKeyframe keyframe;

    [SerializeField] private Button deleteButton;

    private void Start()
    {
        deleteButton.onClick.AddListener(OnDeletePressed);
    }

    public void Init(CameraKeyframe keyframeData)
    {
        keyframe = keyframeData;
    }

    private void OnDeletePressed()
    {
        TimelineUIController.Instance.DeleteKeyframe(keyframe, this.gameObject);
    }
    
    
}