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

public class UDPSendChannel : MonoBehaviour
{

	private static int localPort;

	IPEndPoint remoteEndPoint;
	UdpClient client;

	// init
	public UDPSendChannel InitChannel(string ip, int port)
	{
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
		client = new UdpClient();

		return this;
	}

	// sendData
	private void sendString(string message)
	{
		try
		{
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, data.Length, remoteEndPoint);
		}
		catch (Exception err)
		{
			print(err.ToString());
		}
	}

	// OnGUI
	private string IP;  // define in init
	private int port;  // define in init
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
					InitChannel (IP, port);
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