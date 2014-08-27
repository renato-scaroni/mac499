using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
    private GameObject yesButton;
    private GameObject noButton;

	// Use this for initialization
	void Start () {
        yesButton = GameObject.Find("YesButton");
        noButton = GameObject.Find("NoButton");
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(2))
            print("oi");



	}
}
