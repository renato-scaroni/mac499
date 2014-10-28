using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class P2PNetworking : MonoBehaviour 
{
	public int defaultBroadcastPort = 8080;
	public float broadcastInterval = 2f;

	UDPListener broadcastListener;
	string lastpacket = "";

	string playerName;

	public delegate void OnInitMode (List<KnownHost> hostsToConnetcTo);
	public static event OnInitMode InitMode;

	public delegate void OnGameMode ();
	public static event OnGameMode GameMode;
	
	Dictionary<string, KnownHost> _knownHosts;
	public Dictionary<string, KnownHost> knownHosts
	{
		get
		{
			return _knownHosts;
		}
	}

	static P2PNetworking _managerInstance;
	static public P2PNetworking instance
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
		UDPReceiveManager.UDPMessageReceived += HandleMsgReceived;
		P2PNetworking.InitMode += BuildConnections;
	}

	void OnDisable()
	{
		UDPReceiveManager.UDPMessageReceived -= HandleMsgReceived;
		P2PNetworking.InitMode -= BuildConnections;
	}

	int secureRepeat = 10;
	void BuildConnections (List<KnownHost> hostsToConnetcTo)
	{
		foreach (KnownHost kh in hostsToConnetcTo)
		{
			if(kh.alive)
			{

				kh.portToListen = UDPReceiveManager.StartListener (IPAddress.Parse(kh.ip));
				for (int i = 0; i < secureRepeat; i++)
					UDPBroadCast.instance.SendString("Connecting\t"+kh.ip+"\t"+kh.portToListen);
			}
		}
		_state = MODE.InitMode;
	}

	void HandleMsgReceived(string msgReceived)
	{
		if(state == MODE.InitMode)
		{
			CompleteOtherHostsDirectConnection(msgReceived);
		}
		if(state == MODE.StandBy)
			ListenForOtherHosts (msgReceived);
	}

	//Direct communication channel creation auxiliar function
	void CompleteOtherHostsDirectConnection(string msgReceived)
	{
		// The correct structure of the messages that should be  
		// sent at this moment is :
		// SENDER\tTYPE\tRECIEVER\tPORT_TO_REPLY

		string [] msgParts = msgReceived.Split('\t');
		IPAddress senderIP = null;
		IPAddress receiverIP = null;
		try
		{
			senderIP = IPAddress.Parse(msgParts[0].ToString());
			receiverIP = IPAddress.Parse(msgParts[2].ToString());
		}
		catch
		{
			Debug.Log("could not resolve sender and receiver");
			return;
		}
		if(InterfacesManager.hostIps.Contains(senderIP))
		{
			return;
		}
		if(msgParts.Length == 4)
		{
			if(msgParts[1] == "Connecting")
			{
				if(InterfacesManager.hostIps.Contains(receiverIP))
				{
					try
					{
						Debug.Log("Recebeu info de "+msgParts[0]);
						knownHosts[msgParts[0]].sendchannel = new UDPSendChannel(msgParts[0],int.Parse(msgParts[3]));
						knownHosts[msgParts[0]].sendchannel.sendString(receiverIP + "\tOK\t"+senderIP+"\t"+knownHosts[msgParts[0]].portToListen);
					}
					catch
					{
						Debug.Log("sender or port not found");
					}
				}
				else
				{
					Debug.Log("retransmitindo msg para "+msgParts[2]);
					UDPBroadCast.instance.SendString(msgReceived);
				}
			}
			if(msgParts[1] == "OK")
			{
				Debug.Log("Recebeu ok de "+msgParts[0]);

				knownHosts[msgParts[0]].directComunicationStablished = true;
			}

		}
		if(msgParts.Length == 3)
		{
			_knownHosts[msgParts[0]].ResetAliveCount();
		}
	}

	// Alive msg handler
	void ListenForOtherHosts(string msgReceived)
	{
		// The correct structure of the messages that should be  
		// sent at this moment is :
		// SENDER\tTYPE\tSENDER_NAME

		string [] msgParts = msgReceived.Split('\t');
		IPAddress senderIP = null;
		try
		{
			 senderIP = IPAddress.Parse(msgParts[0].ToString());
		}
		catch
		{
			Debug.Log("could not resolve sender");
			return;
		}

		if(!InterfacesManager.hostIps.Contains(senderIP))
		{
			if(msgParts[1] == "Connecting")
			{
				List<KnownHost> toConnect = new List<KnownHost> ();
				foreach(KnownHost kh in P2PNetworking.instance.knownHosts.Values)
				{
					if(kh.alive)
						toConnect.Add(kh);
				}        
				InitMode(toConnect);
			}
				
			if(msgParts[1]=="Alive")
			{
				if(!_knownHosts.ContainsKey(msgParts[0]))
				{
					_knownHosts[msgParts[0]] = new KnownHost(msgParts[0], msgParts[2]);//"");
				}
				else
				{
					_knownHosts[msgParts[0]].ResetAliveCount();
				}
				Debug.Log ("Got "+msgReceived+" from " +msgParts[0]);
			}
		}
	}

	void SendAlive(float time)
	{
		UDPBroadCast.instance.SendStringWithDelay ("Alive\t" + playerName, time);//, true);
	}

	public enum MODE {StandBy, GameMode, InitMode};

	static MODE _state = MODE.StandBy;
	public MODE state
	{
		get{return _state;}
	}

	void LifeCycle()
	{
		foreach (KnownHost kh in _knownHosts.Values)
		{
			if(kh.alive)
				kh.Update ();
		}
	}

	//Return 0  indicates GameMode not ready
	//Return 1  indicates GameMode ready
	//Return -1 indicates GameMode failed
	int InitModeUpdate ()
	{
		int count = 0;
		foreach(KnownHost kh in knownHosts)
		{
			if(kh.alive)
			{
				count ++;
				if(kh.directComunicationStablished)
					count --;
			}

		}
		if (count == 0)
			return 1;
		
		return 0;
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
		SendAlive (broadcastInterval);
	}
	
	void Update () 
	{
		switch (_state) 
		{
			case MODE.StandBy:
				LifeCycle();
			break;

	    	case MODE.InitMode:
				int initModeUpdateStatus = InitModeUpdate();
				if( initModeUpdateStatus == 1)
				{
					_state = MODE.GameMode;
					GameMode();
				}
				else if( initModeUpdateStatus == -1)
					_state = MODE.StandBy;
			break;
		}
		//LifeCycle ();
	}

	static public void TriggerInitConnections(List<KnownHost> hostsToConnetcTo)
	{
		InitMode (hostsToConnetcTo);
	}

	void OnApplicationQuit() 
	{
		//Close all channels related to braodcast when the application is closed
		UDPReceiveManager.EveryoneShouldStopListen();
		UDPBroadCast.instance.StopAutoRepeatSend ();
	}
}