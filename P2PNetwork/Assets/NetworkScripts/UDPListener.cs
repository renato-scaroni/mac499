using UnityEngine;
using System.Collections;

using System.Net;
using System.Net.Sockets;

public class UDPListener
{
	// udpclient object
	private UdpClient _client;
	public UdpClient client 
	{ 
		get {return _client;}
		set {_client = value;}
	}
	
	private int _port;
	public int port
	{
		get{return _port;}
		set {_port = value;}
	}
	
	private string _lastReceivedUDPPacket="";
	public string lastReceivedUDPPacket
	{
		get {return _lastReceivedUDPPacket;}
		set {_lastReceivedUDPPacket = value;}
	}
	
	private string _allReceivedUDPPackets="";
	public string allReceivedUDPPackets
	{
		get {return _allReceivedUDPPackets;}
		set {_allReceivedUDPPackets = value;}
	}
	
	private double _timeToWait;
	public double timeToWait
	{
		get {return _timeToWait;}
		set {_timeToWait = value;}
	}
	
	private IPAddress _ip;
	public IPAddress ip
	{
		get {return _ip;}
		set {_ip = value;}
	}
	
	private IPEndPoint _endPoint;
	public IPEndPoint endPoint
	{
		get { return _endPoint;}
		set {_endPoint = value;}
	}

	public bool isBroadcastListener = false;

	public bool receivedMsg;
	
	private const double defaultTimeOut = 15000;
	
	public UDPListener (int portToListen, double time)
	{
		_port = portToListen; 
		_endPoint = new IPEndPoint(IPAddress.Any, portToListen);
		_client = new UdpClient(_endPoint);  
		_timeToWait = time;
		isBroadcastListener = true;
	}
	
	public UDPListener (int portToListen, IPAddress ip, double time)
	{
		_port = portToListen;
		_endPoint = new IPEndPoint(ip, portToListen);
		_client = new UdpClient(_endPoint);  
		_timeToWait = time;
	}
	
	public UDPListener (int portToListen, IPAddress ip)
	{
		_port = portToListen;
		_endPoint = new IPEndPoint(ip, portToListen);
		_client = new UdpClient(_endPoint);  
		_timeToWait = defaultTimeOut;
	}
	
	public UDPListener (int portToListen)
	{
		_port = portToListen;
		_endPoint = new IPEndPoint(IPAddress.Any, portToListen);
		_client = new UdpClient(_endPoint);  
		_timeToWait = defaultTimeOut;
		isBroadcastListener = true;
	}
	
}
