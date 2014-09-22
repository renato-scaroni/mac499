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

public class InterfacesManager : MonoBehaviour 
{
	private static List<NetworkConection> _interfaces;
	public static List<NetworkConection> interfaces
	{
		get 
		{
			if(_interfaces == null)
				InitInterfacesList();
			return _interfaces;
		}
	}

	private static List<IPAddress> _hostIps = null;
	public static List <IPAddress> hostIps
	{
		get 
		{
			if(_hostIps == null)
				return GetHostIps();
			return _hostIps;
		}
	}

	static List<IPAddress> GetHostIps()
	{
		List<IPAddress> ips = new List<IPAddress> ();
		foreach (NetworkConection nc in interfaces)
		{
			ips.Add(nc.ip);
		}
		return ips;
	}

	public static void InitInterfacesList()
	{
		_interfaces = new List<NetworkConection> ();
		InitializeBroadcastIPs ();
	}

	static void InitializeBroadcastIPs()
	{		
		//broadcastIPs = new List <IPAddress>();
		foreach (NetworkInterface iface in NetworkInterface.GetAllNetworkInterfaces())
		{
			//Debug.Log(iface.Description);
			foreach (IPAddressInformation ucastInfo in iface.GetIPProperties().UnicastAddresses)
			{
				if (IPAddress.IsLoopback(ucastInfo.Address) || ucastInfo.Address.AddressFamily == AddressFamily.InterNetworkV6)
					continue;
				
				IPAddress mask = IPAddress.Parse(ReturnSubnetmask(ucastInfo.Address.ToString()));
				byte[] ipAdressBytes = ucastInfo.Address.GetAddressBytes();
				byte[] subnetMaskBytes = mask.GetAddressBytes();
				
				if (ipAdressBytes.Length != subnetMaskBytes.Length) continue;
				
				var broadcast = new byte[ipAdressBytes.Length];
				for (int i = 0; i < broadcast.Length; i++)
				{
					broadcast[i] = (byte)(ipAdressBytes[i] | ~(subnetMaskBytes[i]));
				}
				IPAddress broadcastIP = new IPAddress(broadcast);
				Debug.Log(ucastInfo.Address.AddressFamily.ToString() + "\n" + "\tIP       : " + ucastInfo.Address.ToString()+"\n"+"\tSubnet   : " 
				          + mask.ToString() + "\n" + "\tBroadcast: " + (broadcastIP).ToString());
				NetworkConection newConnection = new NetworkConection ();
				newConnection.broadcast = broadcastIP;
				newConnection.ip = ucastInfo.Address;
				_interfaces.Add(newConnection);
			}
		}
		
	}
	
	static uint  ReturnFirtsOctet(string ipAddress)
	{
		System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
		byte[] byteIP = iPAddress.GetAddressBytes();
		uint ipInUint = (uint)byteIP[0];     
		return ipInUint;
	}
	
	static string ReturnSubnetmask(String ipaddress)	
	{		
		uint firstOctet =  ReturnFirtsOctet(ipaddress);
		
		if (firstOctet >= 0 && firstOctet <= 127)
			return "255.0.0.0";
		else if (firstOctet >= 128 && firstOctet <= 191)
			return "255.255.0.0";
		else if (firstOctet >= 192 && firstOctet <= 223)
			return "255.255.255.0";
		else return "0.0.0.0";
	}
	
	public static IPAddress[] GetAllUnicastAddresses()
	{
		// This works on both Mono and .NET , but there is a difference: it also
		// includes the LocalLoopBack so we need to filter that one out
		List<IPAddress> Addresses = new List<IPAddress>();
		// Obtain a reference to all network interfaces in the machine
		NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface adapter in adapters)
		{
			IPInterfaceProperties properties = adapter.GetIPProperties();
			foreach (IPAddressInformation uniCast in properties.UnicastAddresses)
			{
				if (!IPAddress.IsLoopback(uniCast.Address) && uniCast.Address.AddressFamily!= AddressFamily.InterNetworkV6)
					Addresses.Add(uniCast.Address);
			}
			
		}
		return Addresses.ToArray();
	}
}

public class NetworkConection
{
	public IPEndPoint endPoint;
	public IPAddress ip;
	public IPAddress broadcast;
}