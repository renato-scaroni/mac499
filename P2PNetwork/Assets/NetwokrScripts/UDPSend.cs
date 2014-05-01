/*
		Script based on the examples by la1n
		from unity3D official forum

        -----------------------
        UDP-Send
        -----------------------
		// todo: shutdown thread at the end
    */
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{

	private static int localPort;
	
	// prefs
	private string IP;  // define in init
	public int port;  // define in init

	private string IPInput = "";
	private string portInput = "";

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;
	
	// gui
	string strMessage="";

	private bool sendMode = false;

	public bool debugMode = false;

	public void Start()
	{
		//init(); 
	}
	
	// OnGUI
	void OnGUI()
	{
		if (!debugMode)
		{
			return;
		}

		Rect rectObj=new Rect(40,380,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj,"# UDPSend-Data"
		        ,style);
		
		if (sendMode)
		{
			// ------------------------
			// send it
			// ------------------------
			strMessage=GUI.TextField(new Rect(40,430,140,20),strMessage);	
			if (GUI.Button(new Rect(190,430,40,20),"send"))
			{
				sendString(strMessage+"\n");
			}
		}
		else
		{

			Rect rectIp=new Rect(40,430,200,400);
			GUI.Box(rectIp,"IP"
			        ,style);
			IPInput = GUI.TextField(new Rect(100,430,140,20),IPInput);
			if(IPInput != "")
			{
				IP = IPInput;
			}

			Rect rectPort=new Rect(40,450,200,400);
			GUI.Box(rectPort,"Port"
			        ,style);
			portInput = GUI.TextField(new Rect(100,450,140,20),portInput);
			if(portInput != "")
			{
				port = Convert.ToInt32(portInput);	
			}

			if (GUI.Button(new Rect(250,430,40,20),"send"))
			{
				if((portInput != "") && (IPInput != ""))
				{
					init ();
					sendMode = true;
				}
				else
				{
					Rect warning=new Rect(40,470,200,400);
					GUI.Box(warning,"Type an valid port and IP address"
					        ,style);
				}
			}
		}

	}
	
	// init
	public void init()
	{
		print("UDPSend.init()");
		
		// define
		//IP="127.0.0.1";
		//port=8050;
		
		// ----------------------------
		// Initialize sender
		// ----------------------------
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		client = new UdpClient();
		
		// status
		print("Sending to "+IP+" : "+port);
		print("Testing: nc -lu "+IP+" : "+port);
		
	}

	// sendData
	private void sendString(string message)
	{
		try
		{
			//if (message != "")
			//{
			
			// Encodes Stuff in UTF-8 for sending
			byte[] data = Encoding.UTF8.GetBytes(message);
			
			// Sends Data
			client.Send(data, data.Length, remoteEndPoint);
			//}
		}
		catch (Exception err)
		{
			print(err.ToString());
		}
	}
	
	
	// endless test
	private void sendEndless(string testStr)
	{
		do
		{
			sendString(testStr);
		}
		while(true);
		
	}
	
}