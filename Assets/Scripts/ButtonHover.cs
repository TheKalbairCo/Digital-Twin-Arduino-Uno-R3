using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Renderer cubeRenderer;
    private Vector3 originalLocalPosition;
    private Vector3 originalLocalScale;
    private Color originalColor;

    // Public fields for customization in the Inspector
    [SerializeField] private float hoverHeight = 0.5f;
    [SerializeField] private float sizeMultiplier = 1.2f;
    [SerializeField] private Color hoverColor = Color.green;
    [SerializeField] private Material firstMaterial;  // Material to switch to on first click
    [SerializeField] private Material secondMaterial; // Material to switch to on second click
    [SerializeField] private CubeMaterialChanger targetCube; // Reference to the second cube

    private bool isClicked = false;
    private bool isHovering = false;

    private void Start()
    {
        // Store the initial local position, scale, and color of the cube
        originalLocalPosition = transform.localPosition;
        originalLocalScale = transform.localScale;
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked) // Only apply hover effects if the button isn't clicked
        {
            isHovering = true;
            // Change color, hover up, and increase size when the mouse enters the cube
            cubeRenderer.material.color = hoverColor;
            transform.localPosition = originalLocalPosition + new Vector3(0, hoverHeight, 0);
            transform.localScale = originalLocalScale * sizeMultiplier;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked) // Only reset if the button isn't clicked
        {
            isHovering = false;
            // Reset color, position, and size when the mouse exits the cube
            cubeRenderer.material.color = originalColor;
            transform.localPosition = originalLocalPosition;
            transform.localScale = originalLocalScale;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Toggle the clicked state
        isClicked = !isClicked;

        // Swap materials and reset to original size and position when clicked
        if (isClicked)
        {
            cubeRenderer.material = firstMaterial;
            targetCube.ChangeMaterial(); // Call the CubeMaterialChanger to change the second cube's material
        }
        else
        {
            cubeRenderer.material = secondMaterial;
            // Reset position and size after clicking
            transform.localPosition = originalLocalPosition;
            transform.localScale = originalLocalScale;
        }
    }

    private void Update()
    {
        // Ensures smooth behavior in case of hover issues
        if (isHovering && !isClicked)
        {
            // Keeps the button in the hover state if the mouse is over it but hasn't been clicked
            transform.localPosition = originalLocalPosition + new Vector3(0, hoverHeight, 0);
            transform.localScale = originalLocalScale * sizeMultiplier;
        }
    }
}
