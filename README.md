# Digital Twin Arduino Uno R3

This project demonstrates how to control an Arduino's LED through a Node.js server and synchronize the Arduino's RX and TX LEDs with a virtual representation in Unity. By using a serial connection to communicate with the Arduino, the Node.js server sends commands to toggle the LED state and responds to RX and TX activity, which is mirrored by virtual LEDs in Unity.

## Table of Contents
- [Overview](#overview)
- [Hardware Requirements](#hardware-requirements)
- [Software Requirements](#software-requirements)
- [Project Setup](#project-setup)
  - [Step 1: Arduino Setup](#step-1-arduino-setup)
  - [Step 2: Node.js Setup](#step-2-nodejs-setup)
  - [Step 3: Unity Setup](#step-3-unity-setup)
- [How It Works](#how-it-works)
- [Usage](#usage)
- [Troubleshooting](#troubleshooting)
- [Contributors](#contributors)

## Overview
This project uses a combination of:
1. **Arduino** to physically control an LED and monitor RX and TX data.
2. **Node.js** server to send and receive commands between the Arduino and the Unity app.
3. **Unity** to simulate the Arduino board's LEDs (RX, TX, and Built-In LED) in a virtual 3D environment.

## Hardware Requirements
- **Arduino Uno** or any compatible board.
- **USB cable** to connect the Arduino to your computer.
- **LED** Arduino's Built-in LED
  
## Software Requirements
- **[Arduino IDE](https://www.arduino.cc/en/software)**
- **[Node.js](https://nodejs.org/en/)** (Ensure you have at least Node.js v16+)
- **[Unity](https://unity.com/)** (Version 2021.3 LTS or newer recommended)

## Project Setup

### Step 1: Arduino Setup

#### 1.1. Arduino 
Ensure the Arduino is working
  
#### 1.2. Arduino Sketch

Upload the following Arduino sketch to your Arduino:

```cpp
const int ledPin = 13;
const int txLedPin = 12; // Simulated TX LED Pin
const int rxLedPin = 11; // Simulated RX LED Pin
int incomingByte;

void setup() {
  Serial.begin(115200);
  pinMode(ledPin, OUTPUT);
  pinMode(txLedPin, OUTPUT);
  pinMode(rxLedPin, OUTPUT);

  for (size_t i = 0; i < 10; i++) {
    digitalWrite(ledPin, HIGH);
    delay(50);
    digitalWrite(ledPin, LOW);
    delay(50);
  }
}

void loop() {
  if (Serial.available() > 0) {
    incomingByte = Serial.read();
    
    switch (incomingByte) {
      case 'H':
        digitalWrite(ledPin, HIGH);
        break;
      case 'L':
        digitalWrite(ledPin, LOW);
        break;
      case 'R':
        resetFunc();
        break;
      case 'M':
        Serial.println(String(millis()));
        break;
      default:
        Serial.println("Unknown command");
    }
    
    // Simulate RX LED activity
    digitalWrite(rxLedPin, HIGH);
    delay(50);
    digitalWrite(rxLedPin, LOW);
  }
  
  // Simulate TX LED activity
  digitalWrite(txLedPin, HIGH);
  delay(50);
  digitalWrite(txLedPin, LOW);
}
```

### Step 2: Node.js Setup

#### 2.1. Install Dependencies
First, create a folder for your Node.js server and install the required packages:

```bash
mkdir ArduinoNodeControl
cd ArduinoNodeControl
npm init -y
npm install express serialport body-parser
```

#### 2.2. Node.js Server Code
Create a `server.js` file with the following content:

```javascript
const express = require('express');
const { SerialPort } = require('serialport');
const bodyParser = require('body-parser');

const app = express();
const port = 3000;

const arduinoPort = 'COM3'; // Adjust the COM port as needed
const arduino = new SerialPort({
  path: arduinoPort,
  baudRate: 115200
});

app.use(bodyParser.json());

app.post('/led', (req, res) => {
  const { state } = req.body;

  if (state === 'on') {
    arduino.write('H');
  } else if (state === 'off') {
    arduino.write('L');
  }

  res.sendStatus(200);
});

app.listen(port, () => {
  console.log(`Server running on http://localhost:${port}`);
});
```

#### 2.3. Running the Server

Start the server by running:

```bash
node server.js
```

Make sure your Arduino is connected, and the correct COM port is set in the Node.js script.

### Step 3: Unity Setup

#### 3.1. Unity Scene Setup
1. **Create a 3D model** of the Arduino in Unity (or import a pre-made model).
2. **Add cubes or other 3D objects** to represent the TX, RX, and the main LED on the board.
3. **Assign materials** to the LEDs that will change based on the state.

#### 3.2. Unity Scripts

Create a script named `LEDController.cs` for the virtual LED behavior:

```csharp
using UnityEngine;
using UnityEngine.EventSystems;

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
```

Use this script to control the virtual LEDs in Unity based on data received from the Node.js server.

## How It Works
- **Arduino:** Listens for commands from the Node.js server via serial communication. When it receives a command, it toggles the main LED or sends a status back.
- **Node.js Server:** Acts as a bridge between the Unity app and the Arduino. It sends commands like `H` (high) and `L` (low) to the Arduino to control the LEDs.
- **Unity:** Simulates the Arduino LEDs, receiving feedback from the Node.js server. The virtual LEDs in Unity reflect the current state of the physical Arduino LEDs.

## Usage

1. **Upload the Arduino sketch** to the Arduino.
2. **Run the Node.js server**: `node server.js`.
3. **Run the Unity app** to interact with the virtual LEDs.

You can click on the virtual Arduino LED in Unity to toggle the state, and this will also update the physical LED on the Arduino board.

## Troubleshooting

### Common Issues
1. **"Access denied" on COM port:**
   - Make sure the Arduino is not being used by another application, such as the Arduino IDE.
   - Ensure the correct COM port is set in the Node.js script.

2. **Node.js Server not responding:**
   - Ensure you are running the server with the correct port.
   - Check the connection between Node.js and Arduino, and ensure the serial port settings are correct.

## Contributors
- **Abhimanyu Venu** -  Development, and project setup.
- **Kristian Gunder Kramås** - Arduino Uno R3 3D Model
  
Feel free to contribute or raise issues for further enhancements!

[Youtube Link](https://youtu.be/qbB_WmWwdyU)
