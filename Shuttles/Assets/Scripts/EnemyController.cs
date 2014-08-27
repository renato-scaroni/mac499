using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyController : MonoBehaviour {
	
	
	public Enemy enemyPrefab;
	public Player player;
	List<Enemy> activeEnemies;
    List<Enemy> toRemove;
	private SceneController sceneController;
	
	


	// Use this for initialization
	void Start () 
	{
		sceneController = GameObject.Find("sceneController").GetComponent<SceneController>();
		activeEnemies = new List<Enemy>();
        toRemove = new List<Enemy>();
        enemyPrefab.player = player;
		activeEnemies.Add((Enemy) Instantiate (enemyPrefab));
	}
	
	public void InstantiateNewEnemy ()
	{
		Enemy newEnemy = (Enemy) Instantiate (enemyPrefab);
		activeEnemies.Add(newEnemy);
	}

    public List<Enemy> GetActiveEnemies()
    {
        return activeEnemies;
    }
	
	
	// Update is called once per frame
	void Update () 
	{
        foreach (Enemy e in activeEnemies)
        {
            if (e.WasShot())
            {
                toRemove.Add(e);
            }
        }
        foreach (Enemy e in toRemove)
        {
            activeEnemies.Remove(e);
            InstantiateNewEnemy();
            Destroy(e.gameObject);
        }

        if (toRemove.Count > 0)
            toRemove = new List<Enemy>();
	}
}
