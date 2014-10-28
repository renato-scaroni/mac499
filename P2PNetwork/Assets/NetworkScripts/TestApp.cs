using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

public class TestApp : MonoBehaviour 
{

	void OnGUI()
	{
		Rect textRect=new Rect(40,10,200,400);
		Rect buttonRect=new Rect(320,10,140,40);
		GUIStyle style = new GUIStyle();
		string s = "\nknownHosts: \n";
		switch(P2PNetworking.instance.state)
		{
			case P2PNetworking.MODE.StandBy:
				style.alignment = TextAnchor.UpperLeft;
				foreach (KnownHost kh in P2PNetworking.instance.knownHosts.Values)
				{
					if(kh.alive)
						s += kh.ip + " - " + kh.name +   kh.directComunicationStablished +"\n";
			}
				GUI.Box(textRect, "Programme running for "+Time.time+" seconds\t" + "\n # UPD-Broadcast" + s
				        ,style);
				if(GUI.Button(buttonRect,"Initiate game Mode"))
				{
					List<KnownHost> toConnect = new List<KnownHost> ();
					foreach(KnownHost kh in P2PNetworking.instance.knownHosts.Values)
					{
						if(kh.alive)
							toConnect.Add(kh);
					}        
					P2PNetworking.TriggerInitConnections(toConnect);
				}
			break;
			case P2PNetworking.MODE.InitMode:
				style.alignment = TextAnchor.UpperLeft;
				foreach (KnownHost kh in P2PNetworking.instance.knownHosts.Values)
				{
					if(kh.alive)
						s += kh.ip + " - " + kh.name +   kh.directComunicationStablished +"\n";
				}
				GUI.Box(textRect, "Programme running for "+Time.time+" seconds\t" + "\n # UPD-Broadcast" + s
				        ,style);
				if(GUI.Button(buttonRect,"encerrar app"))
				{
					Application.Quit();
				}
			break;
		}
	}

}