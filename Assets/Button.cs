using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Arduino arduino;
    [SerializeField] Renderer buttonRenderer;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material hoverMaterial;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform ButtonModel;

    bool press = false;
    bool hover = false;

    private void OnMouseEnter()
    {
        buttonRenderer.material = hoverMaterial;
        if (ButtonModel != null) ButtonModel.localScale = Vector3.one * 1.25f;
        hover = true;
    }

    private void OnMouseExit()
    {
        buttonRenderer.material = normalMaterial;
        if (ButtonModel != null) ButtonModel.localScale = Vector3.one * 1f;
        arduino.LEDBuiltIn.SetActive(false);
        hover = false;
        press = false;
    }

    void OnMouseDown()
    {
        arduino.LEDBuiltIn.SetActive(true);
        press = true;
        if (ButtonModel != null) ButtonModel.localScale = new Vector3(1.25f, 0.01f, 1.25f);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (hover)
            {
                if (ButtonModel != null) ButtonModel.localScale = Vector3.one * 1.25f;
            }
            else
            {
                if (ButtonModel != null) ButtonModel.localScale = Vector3.one * 1f;
            }
            if (press)
            {
                arduino.LEDBuiltIn.SetActive(false);
                arduino.ArduinoReset();
            }
            press = false;
        }
        transform.parent.position = parentTransform.position;
        transform.parent.rotation = parentTransform.rotation;
    }
}
