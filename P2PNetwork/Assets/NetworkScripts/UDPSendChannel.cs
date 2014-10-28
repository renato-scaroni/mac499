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

/*************************************************************************/
/* This script is part of the P2PNetwork Plugin for Unity 3D.            */
/* Developed by Renato Scaroni                                           */
/*************************************************************************/

public class UDPSendChannel
{

	private static int localPort;

	IPEndPoint remoteEndPoint;
	UdpClient _client;
	public UdpClient client
	{
		get
		{
			return _client;
		}
	}

	public UDPSendChannel (string ip, int port)
	{
		_ip = ip;
		_port = port;
		InitChannel ();
	}

	// init
	void InitChannel()
	{
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
		_client = new UdpClient();
	}

	// sendData
	public void sendString(string message)
	{
		try
		{
			byte[] data = Encoding.UTF8.GetBytes(message);
			_client.Send(data, data.Length, remoteEndPoint);
		}
		catch (Exception err)
		{
			Debug.Log(err.ToString());
		}
	}

	// OnGUI
	private string _ip;  // define in init
	private int _port;  // define in init
	private bool sendMode = false;
	public bool debugMode = false;
	private string IPInput = "";
	private string portInput = "";
	string strMessage="";
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
				_ip = IPInput;
			}
			
			Rect rectPort=new Rect(40,450,200,400);
			GUI.Box(rectPort,"Port"
			        ,style);
			portInput = GUI.TextField(new Rect(100,450,140,20),portInput);
			if(portInput != "")
			{
				_port = Convert.ToInt32(portInput);	
			}
			
			if (GUI.Button(new Rect(250,430,40,20),"send"))
			{
				if((portInput != "") && (IPInput != ""))
				{
					InitChannel ();
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

}