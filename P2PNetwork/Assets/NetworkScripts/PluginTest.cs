using UnityEngine;
using System.Collections;

public class PluginTest : MonoBehaviour 
{

	void OnGUI()
	{
		Rect rectObj=new Rect(40,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		string s = "\nknownHosts: \n";
		foreach (KnownHost kh in P2PNetworking.managerInstance.knownHosts.Values)
		{
			if(kh.alive)
				s += kh.ip + " - " + kh.name + "\n";
		}
		GUI.Box(rectObj, "Programme running for "+Time.time+" seconds\t" + "\n # UPD-Broadcast" + s
		        ,style);
	}


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
