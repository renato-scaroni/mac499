using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	static EnemyController enemyController;
	static TextMesh score;
	static string baseScoreText;
	static int scoreCount;
	static bool scoreIncremented;
    static Player player;
    static Vector3 playerOrigPos;
    static float totalDeltaTime;
    static string baseLivesText;
    static TextMesh lives;
	
	// Use this for initialization
	void Start () 
	{
		enemyController = GameObject.Find("enemyController").GetComponent<EnemyController>();
		score = GameObject.Find("Score").GetComponent<TextMesh>();
		scoreCount = 0;
		scoreIncremented = true;
		baseScoreText = "Score: ";
        player = GameObject.Find("Player").GetComponent<Player>();
        playerOrigPos = player.transform.position;
        totalDeltaTime = 0;
        baseLivesText = "Lives: ";
        lives = GameObject.Find("LivesCounter").GetComponent<TextMesh>();
        lives.text = baseLivesText + "3";
    }
	
	public void GenerateNewEnemy ()
	{
		enemyController.InstantiateNewEnemy();
	}
	
	static public void IncrementScore ()
	{
		scoreIncremented = true;
		scoreCount += 5;
	}
	
	static public void ResetScore ()
	{
		print ("hey");
		scoreIncremented = true;
		scoreCount = 0;
	}


    static bool PlayerWasHit()
    {
        foreach (Enemy e in enemyController.GetActiveEnemies())
        {
            if(e.renderer.bounds.Intersects(player.renderer.bounds))
            {
                ResetScore();
                player.transform.position = playerOrigPos;
                e.SetShot(true);
                player.DecrementLives();
                if (player.GetLives() >= 0)
                    lives.text = baseLivesText + player.GetLives();
                return true;
            }
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(scoreIncremented)
		{
			score.text = baseScoreText + scoreCount;
			scoreIncremented ^= scoreIncremented;
		}

        if (PlayerWasHit())
        {
            if (player.GetLives() < 0)
            {
                Application.LoadLevel("GameOver");
            }
        }

        if (scoreCount%25 == 0 && scoreCount != 0)
        {
            enemyController.InstantiateNewEnemy();
            IncrementScore();
        }
	}
}
