using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent; // For ConcurrentQueue (thread-safe)

public class Controller : MonoBehaviour
{
    // --- Configuration ---
    public string portName = "COM3"; // Change this to your serial port (e.g., "COM1", "/dev/ttyUSB0")
    public int baudRate = 115200;      // Match this to your device's baud rate
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;
    public int readTimeout = 500;    // Milliseconds for read timeout

    // --- Internal Variables ---
    private SerialPort serialPort;
    private Thread readThread;
    private bool isRunning = false;
    private ConcurrentQueue<string> receivedDataQueue = new ConcurrentQueue<string>(); // Thread-safe queue for incoming data
    public string lastReceivedMessage = ""; // The string variable to store the latest message

    // --- Unity Callbacks ---

    void Start()
    {
        OpenSerialPort();
    }

    void Update()
    {
        // Process received data on the main thread
        while (receivedDataQueue.TryDequeue(out string message))
        {
            lastReceivedMessage = message;
            Debug.Log("Received from serial: " + message);

            // You can now use lastReceivedMessage to update UI, game logic, etc.
            // Example: GetComponent<TextMeshProUGUI>().text = lastReceivedMessage;
        }
    }

    void OnApplicationQuit()
    {
        CloseSerialPort();
    }

    // --- Serial Port Methods ---

    public void OpenSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            Debug.LogWarning("Serial port is already open.");
            return;
        }

        try
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = readTimeout;
            serialPort.NewLine = "\n"; // Important for ReadLine() to work correctly

            serialPort.Open();
            isRunning = true;
            Debug.Log("Serial port opened: " + portName);

            // Start the reading thread
            readThread = new Thread(ReadSerialData);
            readThread.IsBackground = true; // Allows the thread to terminate with the application
            readThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    public void CloseSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            isRunning = false;
            if (readThread != null && readThread.IsAlive)
            {
                readThread.Join(1000); // Wait for the thread to finish (with a timeout)
                if (readThread.IsAlive)
                {
                    readThread.Abort(); // Force terminate if it doesn't close gracefully
                }
            }
            serialPort.Close();
            serialPort.Dispose();
            Debug.Log("Serial port closed.");
        }
    }

    private void ReadSerialData()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // Read a line (until NewLine character, typically '\n')
                string data = serialPort.ReadLine();
                receivedDataQueue.Enqueue(data); // Add data to the thread-safe queue
            }
            catch (TimeoutException)
            {
                // No data received within the timeout period. This is normal.
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading from serial port: " + e.Message);
                isRunning = false; // Stop reading on error
            }
        }
    }

    // Optional: Method to send data (also consider threading for sending in complex scenarios)
    public void WriteSerialData(string message)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(message);
                Debug.Log("Sent to serial: " + message);
            }
            catch (Exception e)
            {
                Debug.LogError("Error writing to serial port: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Serial port not open. Cannot send data.");
        }
    }

    // Helper to get available port names (useful for debugging/UI)
    public static string[] GetPortNames()
    {
        return SerialPort.GetPortNames();
    }
}