using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltInLED : MonoBehaviour
{
    [SerializeField] Arduino arduino;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform LEDModel;

    bool press = false;
    bool hover = false;

    private void OnMouseEnter()
    {
        hover = true;
        if (LEDModel != null) LEDModel.localScale = Vector3.one * 1.5f;
    }

    private void OnMouseExit()
    {
        hover = false;
        if (LEDModel != null) LEDModel.localScale = Vector3.one * 1f;
        press = false;
    }

    void OnMouseDown()
    {
        if (LEDModel != null) LEDModel.localScale = Vector3.one * 1.25f;
        press = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (hover)
            {
                if (LEDModel != null) LEDModel.localScale = Vector3.one * 1.5f;
            }
            else
            {
                if (LEDModel != null) LEDModel.localScale = Vector3.one * 1f;
            }
            if (press)
            {
                arduino.ArduinoLED();
            }
            press = false;
        }
        transform.parent.position = parentTransform.position;
        transform.parent.rotation = parentTransform.rotation;
    }
}

