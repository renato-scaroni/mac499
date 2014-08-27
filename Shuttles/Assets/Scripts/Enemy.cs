using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float minSpeed;
	public float maxSpeed;
	public Player player;
	
	private float currentSpeed;
	private float x, y, z;
	
	private bool shot;
	
	// Use this for initialization
	void Start () 
	{
		SetPositionAndSpeed ();
		shot  = false;
	}
	

	float sides;
	public void SetPositionAndSpeed ()
	{
		currentSpeed = Random.Range (minSpeed, maxSpeed);
		
		x = Random.Range (-6.0f, 6.0f);
		y = 6.0f;
		z = 5.0f;
		transform.position = new Vector3(x, y, z);
		transform.up = player.transform.position - transform.position;
	}

    public void SetShot(bool s)
    {
        shot = s;
    }

	public bool WasShot ()
	{
		return shot;
	}
	
	// Update is called once per frame
	//float nonVisibleTime;
	void Update () 
	{
		float amountToMove = currentSpeed * Time.deltaTime;
		
		transform.Translate (Vector3.up * amountToMove);
		float howMuchToTurn = 1 - Vector3.Dot(transform.up, player.transform.up);
		//transform.RotateAround(transform.position, transform.forward, howMuchToTurn * 10);


		
		if(!renderer.isVisible)
		{
			SetPositionAndSpeed ();
		}
	}
	
	void OnTriggerEnter(Collider otherObject)
	{
        print("Collided against " + otherObject.name);

		if(otherObject.name.Contains("Projectile"))
		{
			SceneController.IncrementScore ();
			shot = true;
		}
	}
}
