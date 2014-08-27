using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float projectileSpeed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float amountToMove = projectileSpeed * Time.deltaTime;
		
		transform.Translate (Vector3.up * amountToMove);
		
		if(transform.position.y >= 6.0f)
			Destroy(this.gameObject);
		
	}
	
	void OnTriggerEnter(Collider otherObject)
	{
		if(otherObject.name != "Player")
			Debug.Log ("Atingiu " + otherObject.name);
		
		if(otherObject.name.Contains("Enemy"))
		{
			Destroy(this.gameObject);
		}
	}
}
