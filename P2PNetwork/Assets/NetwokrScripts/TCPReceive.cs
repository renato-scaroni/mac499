using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class TCPReceive : MonoBehaviour
{
	private System.Net.IPAddress serverAddress;

	private TcpListener listener;

	private TcpClient ourTCP_Client;

	private NetworkStream ourStream;

	public bool debugMode = false;

	private bool receiveMode = false;
	
	private string portInput = "";

	private string lastReceivedPacket;

	private string allReceivedPackets;

	private int Port = 9090; //Port to connect. Default: 9090

	// receiving Thread
	private Thread receiveThread;

	void OnGUI()
	{
		if (!debugMode)
		{
			return;
		}

		Rect rectObj=new Rect(40,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj,"# TCPReceive\n"
		        + "\nLast Packet: \n"+ lastReceivedPacket
		        + "\n\nAll Messages: \n"+ allReceivedPackets
		        ,style);
		if (!receiveMode)
		{

			Rect rectPort=new Rect(40,430,200,400);
			GUI.Box(rectPort,"Port"
			        ,style);

			portInput = GUI.TextField(new Rect(100,430,140,20),portInput);

			if (GUI.Button(new Rect(250,430,45,20),"Recieve"))
			{
				Debug.Log ("Button clicked");
	

				if((portInput != ""))
				{
					Port = Convert.ToInt32(portInput);
					Debug.Log("ok, lets try to Recieve");
					StartListener ();
					receiveMode = true;
				}
				else
				{
					Rect warning=new Rect(40,470,200,400);
					GUI.Box(warning,"Type an valid Port to Recieve to"
					        ,style);
				}
			}
		}
		else
		{
			if (GUI.Button(new Rect(250,430,100,20),"Stop Receiving"))
			{
				receiveMode = false;
			}
		}
	}


	public string GetLocalIPAddress ()
	{
			IPHostEntry host;
			string localIP = "";
			host = Dns.GetHostEntry (Dns.GetHostName ());
			foreach (IPAddress ip in host.AddressList) {
					if (ip.AddressFamily == AddressFamily.InterNetwork) {
							localIP = ip.ToString ();
							break;
					}
			}
			return localIP;
	}

	void Start ()
	{

	}


	volatile bool receiveInited = false;
	void Recieve()
	{
		if (!receiveInited)
		{
			print("receiver not inited");
			return;
		}

		Debug.Log("Starting to Recieve");

		while (receiveMode)
		{
			byte[] data = new byte [ourTCP_Client.ReceiveBufferSize];

			// read the incoming data stream - note that Read() is a blocking call...
			int bytesRead = ourStream.Read (data, 0, System.Convert.ToInt32 (ourTCP_Client.ReceiveBufferSize));

			string read = Encoding.ASCII.GetString (data, 0, bytesRead);

			// echo the data we got to the console until the newline.

			allReceivedPackets += read + "\n";
			lastReceivedPacket = read;

			Debug.Log ("Received : " + read);
		}
		//return read;
	}

	void StartListener ()
	{
		receiveThread = new Thread(new ThreadStart(InitReciever));
		receiveThread.IsBackground = true;
		receiveThread.Start();		

	}

	void InitReciever ()
	{
			Debug.Log ("Starting Listener");

			// Our local IP address - you'll be wanting to change this...
			serverAddress = IPAddress.Parse(GetLocalIPAddress());
			
			// Start listening for connections on our IP address + Our Port number 
			listener = new TcpListener (serverAddress, Port);
			listener.Start ();
			
			// Is someone trying to call us? Well answer!
			ourTCP_Client = listener.AcceptTcpClient ();
			
			//A network stream object. We'll use this to send and receive our data, so create a buffer for it...
			ourStream = ourTCP_Client.GetStream ();
			receiveInited = true;

			Recieve();

			Debug.Log ("Listener started");
	}
}
