using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/*************************************************************************/
/* This script is part of the P2PNetwork Plugin for Unity 3D.            */
/* Developed by Renato Scaroni                                           */
/*************************************************************************/

public class UDPReceiveManager
{
	static List<Thread> receiveThreads;

	
	static Dictionary <int, UDPListener> listeners;
	
	private static bool keepListening = true;
	
	public delegate void OnUDPMessageReceived (string msgReceived);
	public static event OnUDPMessageReceived UDPMessageReceived;
	
/*********************************************************************************/
// Access Methods

	public static Dictionary<int, UDPListener> GetActiveListeners ()
	{

		if(listeners == null)
			listeners = new Dictionary<int, UDPListener> ();

		return listeners;
	}

	public static void EveryoneShouldListen()
	{
		keepListening = true;
	}

	public static void EveryoneShouldStopListen()
	{
		keepListening = false;
	}

/*********************************************************************************/


/*********************************************************************************/
// Listener thread init and management functions

	//returns the port which it used
	public static int StartListener(IPAddress ip)
	{
		//Debug.Log("UDPReceiveManager.StartListener()");
		
		if(listeners == null)
			listeners = new Dictionary<int, UDPListener> ();
		
		UDPListener newListener = new UDPListener (ip);
		int port = newListener.port;
		listeners.Add (port, newListener);

		if (receiveThreads == null)
			receiveThreads = new List<Thread> ();
		
		Thread receiveThread = new Thread(
			new ThreadStart(() => ManageListening(listeners[port])));
		receiveThread.IsBackground = true;
		receiveThreads.Add (receiveThread);
		receiveThread.Start();	

		return port;
	}

	//Initalizes a socket to listen to an specific ip at as specific given port
	public static void StartListener(int port, IPAddress ip)
	{
//		Debug.Log("UDPSend.StartListener()");

		if(listeners == null)
			listeners = new Dictionary<int, UDPListener> ();

		if(!listeners.ContainsKey(port))
		{
			UDPListener newListener = new UDPListener (port, ip);
			listeners.Add (port, newListener);
		}

		if (receiveThreads == null)
			receiveThreads = new List<Thread> ();

		Thread receiveThread = new Thread(
			new ThreadStart(() => ManageListening(listeners[port])));
		receiveThread.IsBackground = true;
		receiveThreads.Add (receiveThread);
		receiveThread.Start();		
	}

	//Initalizes a socket to listen to any at as specific given port
	public static void StartListener(int port)
	{
		Debug.Log("UDPSend.StartListener()");

		if(listeners == null)
			listeners = new Dictionary<int, UDPListener> ();

		if(!listeners.ContainsKey(port))
		{
			UDPListener newListener = new UDPListener (port);
			listeners.Add (port, newListener);
		}

		if (receiveThreads == null)
			receiveThreads = new List<Thread> ();

		Thread receiveThread = new Thread(
			new ThreadStart(() => ManageListening(listeners[port])));
		receiveThread.IsBackground = true;
		receiveThreads.Add (receiveThread);
		receiveThread.Start();		
	}

	public static void ManageListening (UDPListener listener)
	{
		listener.receivedMsg = true;
		Mutex mutex = new Mutex();
		listener.mutex = mutex;
		while (keepListening)
		{	
			if(listener.receivedMsg)
			{
				mutex.WaitOne ();
				listener.receivedMsg = false;
				mutex.ReleaseMutex();
				listener.client.BeginReceive(new AsyncCallback(Listen), listener);
			}	
		}
		Debug.Log ("Quitting listener");
	}

	// receive thread
	public static void Listen(IAsyncResult ar)
	{
		UDPListener l = (UDPListener)(ar.AsyncState);
		UdpClient c = (UdpClient)l.client;
		IPEndPoint e = (IPEndPoint)l.endPoint;
		Mutex mutex = (Mutex)l.mutex;

		Byte[] receiveBytes = c.EndReceive(ar, ref e);
		string msg = Encoding.ASCII.GetString(receiveBytes);

		string[] msgParts = msg.Split('\t');

		if(l.boundIP == "" || msgParts[0] == l.boundIP)
			UDPReceiveManager.UDPMessageReceived (msg);

		l.lastReceivedUDPPacket = msg;
		l.allReceivedUDPPackets += l.lastReceivedUDPPacket+"\n";

		mutex.WaitOne ();
		l.receivedMsg = true;
		mutex.ReleaseMutex ();

	}
/*********************************************************************************/
}