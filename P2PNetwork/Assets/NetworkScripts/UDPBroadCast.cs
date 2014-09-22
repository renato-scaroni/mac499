using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections.Generic;

/*************************************************************************/
/* This script is part of the P2PNetwork Plugin for Unity 3D.            */
/* Developed by Renato Scaroni                                           */
/*************************************************************************/


// Module responsible for BroadCast using UDP Protocol
public class UDPBroadCast : MonoBehaviour {

	private static int localPort;
	
	int port = 0;
	
	private string portInput = "";
	
	UdpClient client;

	string strMessage="";
	
	private bool sendMode = false;
	
	public bool debugMode = false;
	
	private static UDPBroadCast _instance;
	public static UDPBroadCast instance
	{
		get 
		{
			if(_instance == null)
			{
				_instance = CreateUDPBroadcastManager();
			}
			return _instance;
		}
	}

	public static UDPBroadCast CreateUDPBroadcastManager ()
	{
		if (_instance != null)
			return _instance;

		GameObject go = new GameObject ("UDPBroadcastManager");
		_instance = go.AddComponent<UDPBroadCast> ();
			return _instance;
	}
	
	public static UDPBroadCast CreateUDPBroadcastManager (int port)
	{
		if (_instance != null)
			return _instance;
		
		GameObject go = new GameObject ("UDPBroadcastManager");
		_instance = go.AddComponent<UDPBroadCast> ();
		_instance.Init (port);
		return _instance;
	}

	void Start()
	{
	}

	bool autoSend = false;
	// init
	public void Init(int port)
	{
		if(port == 0)
		{
			Debug.Log ("Please Set a port to BroadCast");
			return;
		}
		foreach (NetworkConection nc in InterfacesManager.interfaces)
			nc.endPoint = new IPEndPoint (nc.broadcast, port);
		client = new UdpClient();
	}

	public string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}
	
	public void SendString(string message)
	{
		foreach(NetworkConection nc in InterfacesManager.interfaces)
		{
			try
			{
				byte[] data = Encoding.UTF8.GetBytes(nc.ip+"\t"+message);
				client.Send(data, data.Length, nc.endPoint);					
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}

	float delayTime = 1;
	List<string> msgToSendDelay = new List<string>();
	bool repeat = false;
	public void SendStringWithDelay(string message, float delay, bool _repeat = false)
	{
		msgToSendDelay.Add (message);
		delayTime = delay;
	}

	public void StopAutoRepeatSend()
	{
		repeat = false;
	}

	float time = 0;
	void Update()
	{
		if(time >= delayTime)
		{
			print ("sending delayed msg");
			if(msgToSendDelay.Count > 0)
				SendString(msgToSendDelay[0]);
			time = 0;
		}
		else
		{
			time += Time.deltaTime;
		}
	}


//	void OnGUI()
//	{
//		Rect rectObj=new Rect(40,10,200,400);
//		GUIStyle style = new GUIStyle();
//		style.alignment = TextAnchor.UpperLeft;
//		
//		if (autoSend) 
//		{
//			GUI.Box(rectObj,"# UPD-Broadcast testing... msg "+ msgSent+ " of "+msgToSend+" sent."
//			        ,style);
//			return;
//		}
//		string s = "\nips: \n";
//		foreach (NetworkConection nc in InterfacesManager.interfaces)
//			s += nc.ip.ToString () + "\n";
//		
//		GUI.Box(rectObj,"# UPD-Broadcast" + s
//		        ,style);
//		if (sendMode)
//		{
//			strMessage=GUI.TextField(new Rect(40,430,140,20),strMessage);	
//			if (GUI.Button(new Rect(190,410,80,20),"autoSendTest"))
//			{
//				sendStringWithDelay(strMessage, 1000, 10);
//				autoSend = true;
//			}
//			
//			if (GUI.Button(new Rect(190,430,40,20),"send"))
//			{
//				SendString(strMessage+"\n");
//			}
//		}
//		else
//		{
//			
//			Rect rectIp=new Rect(40,430,200,400);
//			GUI.Box(rectIp,"IP 255.255.255.255"
//			        ,style);
//			
//			Rect rectPort=new Rect(40,450,200,400);
//			GUI.Box(rectPort,"Port"
//			        ,style);
//			portInput = GUI.TextField(new Rect(100,450,140,20),portInput);
//			if(portInput != "")
//			{
//				port = Convert.ToInt32(portInput);	
//			}
//			
//			if (GUI.Button(new Rect(250,430,40,20),"send"))
//			{
//				if((portInput != ""))
//				{
//					Init (port);
//					sendMode = true;
//				}
//				else
//				{
//					Rect warning=new Rect(40,470,200,400);
//					GUI.Box(warning,"Type an valid port and IP address"
//					        ,style);
//				}
//			}
//		}
//		
//	}	
//
}