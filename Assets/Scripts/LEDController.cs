using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections;

public class LEDController : MonoBehaviour, IPointerClickHandler
{
    private string serverUrl = "http://localhost:3000/led"; // URL of your Node.js server
    private bool isLEDOn = false; // Track the LED state

    public void OnPointerClick(PointerEventData eventData)
    {
        isLEDOn = !isLEDOn; // Toggle the LED state
        StartCoroutine(SendRequest(isLEDOn ? "on" : "off"));
    }

    IEnumerator SendRequest(string state)
    {
        string json = "{\"state\":\"" + state + "\"}";
        using (UnityWebRequest www = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Request sent successfully: " + www.downloadHandler.text);
            }
        }
    }
}
