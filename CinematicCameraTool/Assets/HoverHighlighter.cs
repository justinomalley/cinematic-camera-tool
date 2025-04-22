using UnityEngine;

public class HoverHighlighter : MonoBehaviour
{
    [System.Serializable]
    public class HighlightGroup
    {
        public Renderer[] renderers;
        public Material highlightMaterial;

        [HideInInspector] public int targetMaterialIndex = 0;
        [HideInInspector] public Material originalMaterial;
    }

    [SerializeField] private HighlightGroup[] highlightGroups;

    private bool isHighlighted = false;

    void Start()
    {
        foreach (var group in highlightGroups)
        {
            if (group.renderers.Length > 0)
            {
                var refRenderer = group.renderers[0];
                if (refRenderer != null && refRenderer.materials.Length > 0)
                {
                    group.originalMaterial = refRenderer.materials[0]; // Assume index 0 is the shared one
                    group.targetMaterialIndex = 0;

                    // OPTIONAL: auto-detect correct index
                    for (int i = 0; i < refRenderer.materials.Length; i++)
                    {
                        if (refRenderer.sharedMaterials[i].name.StartsWith(group.highlightMaterial.name.Replace("Highlight", "")))
                        {
                            group.originalMaterial = refRenderer.materials[i];
                            group.targetMaterialIndex = i;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void SetHighlight(bool on)
    {
        if (on == isHighlighted) return;
        isHighlighted = on;

        foreach (var group in highlightGroups)
        {
            foreach (var renderer in group.renderers)
            {
                var materials = renderer.materials;

                if (group.targetMaterialIndex < materials.Length)
                {
                    materials[group.targetMaterialIndex] = on
                        ? group.highlightMaterial
                        : group.originalMaterial;

                    renderer.materials = materials;
                }
            }
        }
    }
}
