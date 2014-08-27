using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float turningSpeed;
	public float aheadSpeed;
	public Projectile projectilePrefab;
    private int lives;
	private bool startedToFire;
	private Vector3 initRotation;

	// Use this for initialization
	void Start () 
	{
		transform.Translate (new Vector3 (0.0f, 0.0f, 0.0f));
		startedToFire = false;
        lives = 3;
	}

    public int GetLives()
    {
        return lives;
    }

    public void DecrementLives()
    {
        lives -= 1;
    }

	// Update is called once per frame
	void Update () 
	{
		//move Player
		float amountToTurn = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime*(-1);
		float speedBoost = Input.GetAxis("Vertical") * Time.deltaTime * 2;

		if(Input.acceleration.x != 0)
		{
			amountToTurn = Input.acceleration.x* 1.1f * turningSpeed * Time.deltaTime*(-1);
		}

		if(Input.acceleration.y != 0)
		{
			amountToTurn = Input.acceleration.y * Time.deltaTime * 2;
		}

		//transform.Translate (Vector3.right * amountToTurn);
		transform.Translate (Vector3.down * (aheadSpeed + speedBoost));
		//transform.Rotate(transform.forward * amountToTurn);
		transform.RotateAround(transform.position, transform.forward, amountToTurn*15);

		//Wrap
		if(transform.position.x <= -7.0)
			transform.position = new Vector3(7.0f, transform.position.y, transform.position.z);
		else if(transform.position.x >= 7.0)
			transform.position = new Vector3(-7.0f, transform.position.y, transform.position.z);
	
		//Fire!!!
		if(Input.GetKeyDown ("space") || (startedToFire && Input.GetTouch(0).phase == TouchPhase.Ended))
		{
			Instantiate (projectilePrefab, transform.position, transform.rotation);
			startedToFire = false;
		}
		
		if(Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			startedToFire = true;
		}

	}
	
	void OnTriggerEnter(Collider otherObject)
	{
		print ("Collided against "+otherObject.name);
		
		if(otherObject.name.Contains("Enemy"))
		{
			//SceneController.ResetScore();
		}
	}

}
