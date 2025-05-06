using UnityEngine;
using UnityEngine.UI;

public class ViewToggle : MonoBehaviour
{
    [Header("UI Groups")]
    public GameObject editorUI;
    public GameObject previewUI;

    [Header("Buttons")]
    public Button editorButton;
    public Button previewButton;
    
    [Header("Cameras")]
    public Camera editorCamera;
    public Camera cinematicCamera;

    [Header("Active Button Colors")]
    public Color activeColor = new Color(1f, 0.8f, 0.2f); // yellowish
    public Color inactiveColor = Color.white;

    private RenderTexture previewTexture;

    private void Start() {
        previewTexture = cinematicCamera.targetTexture;
        // Default to editor view at start
        SetEditorView();
    }

    public void SetEditorView()
    {
        editorUI.SetActive(true);
        previewUI.SetActive(false);

        editorCamera.enabled = true;
        cinematicCamera.targetTexture = previewTexture;

        UpdateButtonStates(editorButton, previewButton);
    }

    public void SetPreviewView()
    {
        editorUI.SetActive(false);
        previewUI.SetActive(true);

        editorCamera.enabled = false;
        cinematicCamera.targetTexture = null;

        UpdateButtonStates(previewButton, editorButton);
    }

    private void UpdateButtonStates(Button active, Button inactive)
    {
        active.GetComponent<Image>().color = activeColor;
        inactive.GetComponent<Image>().color = inactiveColor;
    }
}