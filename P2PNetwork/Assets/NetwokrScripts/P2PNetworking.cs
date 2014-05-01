using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class P2PNetworking : MonoBehaviour 
{
	//private SocketPermission permission;
	private Socket listener;
	private IPHostEntry host;
	private IPAddress ipAddr;
	private IPEndPoint ipEndPoint;

	// Use this for initialization
	void Start () 
	{
		//permission = new SocketPermission(NetworkAccess.Accept, 
		//                                  TransportType.Tcp,
		//                                  "",
		//                                  SocketPermission.AllPorts);
//		host = Dns.GetHostEntry("");
//		ipAddr = host.AddressList[0];
//		ipEndPoint = new IPEndPoint(ipAddr, 4510);
//		listener = Socket(ipAddr.AddressFamily,
//		                  SocketType.Stream,
//		                  ProtocolType.Tcp);
//		listener.Bind(ipEndPoint);
//		Debug.Log(ipAddr.ToString());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}