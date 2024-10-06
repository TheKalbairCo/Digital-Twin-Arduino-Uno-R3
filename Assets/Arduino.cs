using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arduino : MonoBehaviour
{
    public GameObject LEDTX;
    public GameObject LEDRX;
    public GameObject LEDOn;
    public GameObject LEDBuiltIn;
    public string host = "http://localhost:8080";

    bool busy = false;

    public void ArduinoReset()
    {
        StartCoroutine(Reset());
    }

    public void ArduinoLED()
    {
        StartCoroutine(LED());
    }

    public void ArduinoMillis()
    {
        StartCoroutine(Millis());
    }

    IEnumerator Millis()
    {
        // TODO
        yield break;
    }

    IEnumerator LED()
    {
        if (busy) yield break;

        busy = true;
        UnityWebRequest led = UnityWebRequest.PostWwwForm(host + "/led/" + (LEDBuiltIn.activeSelf ? "0" : "1"), "");
        yield return led.SendWebRequest();

        if (led.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(led.error);
        }
        else
        {
            LEDRX.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            LEDRX.SetActive(false);
            LEDBuiltIn.SetActive(!LEDBuiltIn.activeSelf);
        }

        busy = false;
    }

    IEnumerator Reset()
    {
        busy = true;

        UnityWebRequest reset = UnityWebRequest.PostWwwForm(host + "/reset", "");
        yield return reset.SendWebRequest();

        if (reset.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(reset.error);
            yield break;
        }
        else
        {
            LEDRX.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            LEDRX.SetActive(false);
            for (int i = 0; i < 25; i++)
            {
                LEDBuiltIn.SetActive(!LEDBuiltIn.activeSelf);
                yield return new WaitForSeconds(0.0425f);
            }
            LEDBuiltIn.SetActive(false);
            busy = false;
        }

    }

    void Start()
    {

    }

    void Update()
    {

    }
}
