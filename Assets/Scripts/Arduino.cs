using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

//Code van https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-7.0
//Niet door mij geschreven!
public class Arduino : MonoBehaviour
{
    public string portName = "COM0";
    public int baudRate;
    public UnityEvent<string> OnCommunicationReceived = new UnityEvent<string>();
    public static bool doContinue;
    public static SerialPort serialPort;
    private static Action<string> InternalBoi;
    private Thread readThread;

    // Start is called before the first frame update
    void Awake()
    {
        serialPort = new SerialPort();
        serialPort.PortName = portName;
        serialPort.BaudRate = baudRate;
        serialPort.ReadTimeout = 500;
        serialPort.WriteTimeout = 500;
        serialPort.Open();
        doContinue = true;
        readThread = new Thread(Read);
        readThread.Start();
        InternalBoi = (value) => OnCommunicationReceived.Invoke(value);
    }

    private void OnApplicationQuit()
    {
        doContinue = false;
        readThread?.Join();
        serialPort?.Close();
    }

    public static void Read()
    {
        while (doContinue)
        {
            try
            {
                string message = serialPort.ReadLine(); //Lees een berichtje van de arduino
                InternalBoi?.Invoke(message);
            }
            catch (TimeoutException) { }
        }
    }
}
