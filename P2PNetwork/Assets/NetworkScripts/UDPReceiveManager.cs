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

public class UDPReceiveManager : MonoBehaviour
{
	static List<Thread> receiveThreads;

	
	static Dictionary <int, UDPListener> listeners;
	
	private static bool keepListening = true;
	
	public delegate void OnUDPMessageReceived (UDPListener listener);
	public static event OnUDPMessageReceived UDPMessageReceived;

	public static Mutex mutex = new Mutex();

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

	public static void StartListener(int port, IPAddress ip)
	{
		print("UDPSend.StartListener()");

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

	public static void StartListener(int port)
	{
		print("UDPSend.StartListener()");

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

		Byte[] receiveBytes = c.EndReceive(ar, ref e);
		UDPReceiveManager.UDPMessageReceived (l);
	
		mutex.WaitOne ();
		l.receivedMsg = true;
		mutex.ReleaseMutex ();

		l.lastReceivedUDPPacket = Encoding.ASCII.GetString(receiveBytes);
		l.allReceivedUDPPackets += l.lastReceivedUDPPacket+"\n";
	}
/*********************************************************************************/
}