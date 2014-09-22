using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class ReceiveTest : MonoBehaviour 
{
	public bool udp;

	bool receiveMode;
	string portInput = "";
	UDPReceiveManager udpRecManager;
	
	void UDPRecTest ()
	{
		Rect rectObj=new Rect(340,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		
		if(UDPReceiveManager.GetActiveListeners().Count > 0)
		{
			
			GUI.Box(rectObj,"# UDPReceive\n"
			        + "\nLast Packet: \n" + UDPReceiveManager.GetActiveListeners()[8080].lastReceivedUDPPacket
			        + "\n\nAll Messages: \n" + UDPReceiveManager.GetActiveListeners()[8080].allReceivedUDPPackets
			        ,style);
		}
		
		if (!receiveMode)
		{
			
			Rect rectPort=new Rect(340,430,200,400);
			GUI.Box(rectPort,"Port"
			        ,style);
			portInput = GUI.TextField(new Rect(400,430,140,20),portInput);
			int port = 8080;
			if(portInput != "")
			{
				port = Convert.ToInt32(portInput);	
			}
			
			if (GUI.Button(new Rect(550,430,45,20),"listen"))
			{
				if((portInput != ""))
				{
					receiveMode = true;
					UDPReceiveManager.StartListener (port);
					print ("Got port input");
				}
				else
				{
					Rect warning=new Rect(340,470,200,400);
					GUI.Box(warning,"Type an valid port and IP address"
					        ,style);
				}
			}
		}
		else
		{
			if (GUI.Button(new Rect(550,430,100,20),"Stop Receiving"))
			{
				receiveMode = false;
				UDPReceiveManager.EveryoneShouldStopListen();
			}
		}

	}

	void OnGUI()
	{
		if (udp)
			UDPRecTest ();
	}

}