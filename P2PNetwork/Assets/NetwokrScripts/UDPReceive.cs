/*
		Script based on the examples by la1n
		from unity3D official forum

        -----------------------
        UDP-Receive (send to)
        -----------------------
    */
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {
	
	// receiving Thread
	Thread receiveThread;
	
	// udpclient object
	UdpClient client;
	
	// public
	// public string IP = "127.0.0.1"; default local
	public int port; // define > StartListener

	public string portInput = "";

	// infos
	public string lastReceivedUDPPacket="";
	public string allReceivedUDPPackets=""; // clean up this from time to time!
	
	private volatile bool receiveMode = false;

	public bool debugMode = false;

	// start from unity3d
	public void Start()
	{
		
		//StartListener(); 
	}
	
	// OnGUI
	void OnGUI()
	{
		if (!debugMode)
		{
			return;
		}

		Rect rectObj=new Rect(40,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj,"# UDPReceive\n"
		        + "\nLast Packet: \n"+ lastReceivedUDPPacket
		        + "\n\nAll Messages: \n"+allReceivedUDPPackets
		        ,style);
		if (!receiveMode)
		{

			Rect rectPort=new Rect(40,430,200,400);
			GUI.Box(rectPort,"Port"
			        ,style);
			portInput = GUI.TextField(new Rect(100,430,140,20),portInput);
			if(portInput != "")
			{
				port = Convert.ToInt32(portInput);	
			}
			
			if (GUI.Button(new Rect(250,430,45,20),"listen"))
			{
				if((portInput != ""))
				{
					StartListener ();
					receiveMode = true;
				}
				else
				{
					Rect warning=new Rect(40,470,200,400);
					GUI.Box(warning,"Type an valid port and IP address"
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
	
	// StartListener
	private void StartListener()
	{
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		print("UDPSend.StartListener()");
		
		// define port
		//port = 8050;

		//Starting receive thread
		receiveThread = new Thread(
			new ThreadStart(Listen));
		receiveThread.IsBackground = true;
		receiveThread.Start();		
	}

	// receive thread
	private  void Listen()
	{
		
		client = new UdpClient(port);
		while (receiveMode)
		{
			try
			{
				// Initializes the endpoint and awaits for data
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				
				// Converts From bytes to UTF8
				string text = Encoding.UTF8.GetString(data);

				print(">> " + text);
				
				// latest UDPpacket
				lastReceivedUDPPacket=text;
				
				// ....
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}
	
	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket()
	{
		allReceivedUDPPackets="";
		return lastReceivedUDPPacket;
	}
}