using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class P2PNetworking : MonoBehaviour 
{
	public int defaultBroadcastPort = 8080;

	UDPListener broadcastListener;
	string lastpacket = "";

	string playerName;

	public delegate void OnInitMode (List<KnownHost> hostsToConnetcTo);
	public static event OnInitMode InitMode;

	Dictionary<string, KnownHost> _knownHosts;
	public Dictionary<string, KnownHost> knownHosts
	{
		get
		{
			return _knownHosts;
		}
	}

	static P2PNetworking _managerInstance;
	static public P2PNetworking managerInstance
	{
		get
		{
			if(_managerInstance == null)
				_managerInstance = GameObject.FindObjectOfType(typeof(P2PNetworking)) as P2PNetworking;
			return _managerInstance;
		}
	}

	void OnEnable()
	{
		UDPReceiveManager.UDPMessageReceived += ListenForOtherHosts;
		P2PNetworking.InitMode += BuildConnections;
	}

	void OnDisable()
	{
		UDPReceiveManager.UDPMessageReceived -= ListenForOtherHosts;
		P2PNetworking.InitMode -= BuildConnections;
	}
	int lastUsedPort = 9601;
	void BuildConnections (List<KnownHost> hostsToConnetcTo)
	{
		foreach (KnownHost kh in hostsToConnetcTo)
		{
			if(kh.alive)
			{
				lastUsedPort ++;
				UDPReceiveManager.StartListener (lastUsedPort, IPAddress.Parse(kh.ip));
				for (int i = 0; i < 10; i++)
					UDPBroadCast.instance.SendString("Connecting\t"+kh.ip+"\t"+lastUsedPort);
			}
		}
	}

	void ListenForOtherHosts(UDPListener listener)
	{
		string [] msgParts = listener.lastReceivedUDPPacket.Split('\t');
		IPAddress senderIP = null;
		try
		{
			 senderIP = IPAddress.Parse(msgParts[0].ToString());
		}
		catch
		{
		}
		if(senderIP != null && !InterfacesManager.hostIps.Contains(senderIP))
		{
			if(!_knownHosts.ContainsKey(msgParts[0]))
			{
				_knownHosts[msgParts[0]] = new KnownHost(msgParts[0], msgParts[2]);//"");
			}
			else
			{
				_knownHosts[msgParts[0]].ResetAliveCount();
			}
			Debug.Log ("Got message from " +msgParts[0]);
		}
		lastpacket = listener.lastReceivedUDPPacket;
	}

	void SendAlive()
	{
		UDPBroadCast.instance.SendStringWithDelay ("Alive\t"+playerName, 2f, true);
	}

	enum MODE {StandBy, GameMode, InitMode};

	MODE state = MODE.StandBy;

	void StandByMode()
	{
		foreach (KnownHost kh in _knownHosts.Values)
		{
			if(kh.alive)
				kh.Update ();
		}
	}

	void Start () 
	{
		//Init some reference holders
		playerName = UnityEngine.SystemInfo.deviceName;
		_knownHosts = new Dictionary<string, KnownHost> ();
		InterfacesManager.InitInterfacesList();

		//Init Broadcast send and receive channels
		UDPBroadCast.CreateUDPBroadcastManager (defaultBroadcastPort);
		UDPReceiveManager.StartListener (defaultBroadcastPort);
		broadcastListener = UDPReceiveManager.GetActiveListeners () [defaultBroadcastPort];

		//Initiate periodic alive message
		SendAlive ();
	}

	void Update () 
	{
		switch (state) 
		{
			case MODE.StandBy:
				StandByMode();
				break;
		}
	}
	
	void OnApplicationQuit() 
	{
		//Close all channels related to braodcast when the application is closed
		UDPReceiveManager.EveryoneShouldStopListen();
		UDPBroadCast.instance.StopAutoRepeatSend ();
	}
}