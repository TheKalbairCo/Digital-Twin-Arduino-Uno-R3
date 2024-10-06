using UnityEngine;

public class CubeMaterialChanger : MonoBehaviour
{
    private Renderer cubeRenderer;
    private Material originalMaterial;

    // Public fields for customization in the Inspector
    [SerializeField] private Material newMaterial;
    [SerializeField] private float materialChangeDuration = 1f; // Duration to change the material (1 second)

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalMaterial = cubeRenderer.material; // Store the original material
    }

    // This function will be called by the ButtonHover script when the button is clicked
    public void ChangeMaterial()
    {
        // Change to the new material
        cubeRenderer.material = newMaterial;

        // Invoke a method to revert back to the original material after the set duration
        Invoke(nameof(ResetMaterial), materialChangeDuration);
    }

    private void ResetMaterial()
    {
        // Revert to the original material after the time delay
        cubeRenderer.material = originalMaterial;
    }
}
