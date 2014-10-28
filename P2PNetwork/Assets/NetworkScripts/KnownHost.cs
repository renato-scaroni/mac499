using UnityEngine;
using System.Collections;

public class KnownHost
{
	public bool alive = true;
	public float totalAliveCount = 15f;
	public float aliveCount;

	public string ip;
	public int portToListen;
	public bool directComunicationStablished = false;
	public UDPSendChannel sendchannel;

	public string name;

	public KnownHost(string _ip, string _name)
	{
		ip = _ip;
		name = _name;
	}

	public void ResetAliveCount()
	{
//		Debug.Log ("Resetting count for "+ ip);
		aliveCount = totalAliveCount;
		alive = true;
	}

	// Update is called once per frame
	public void Update () 
	{
		if(aliveCount >= 0)
		{
			aliveCount -= Time.deltaTime;
		}
		else
		{
			alive = false;
		}
	}
}