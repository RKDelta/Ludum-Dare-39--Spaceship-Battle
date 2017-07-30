using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; protected set; }

    public bool isPaused;

    public GameObject[] enemyPrefabs;

    List<Enemy> enemies;

    public Player player;

	void Start ()
    {
        this.enemies = new List<Enemy>();

		if (Instance != null)
        {
            Debug.LogError("There should be no more than one GameController in the scene. There are atleast two.");
        }

        Instance = this;
	}
	
	void FixedUpdate ()
    {
		if (Random.Range(0, 100) == 0 && this.enemies.Count < 5)
        {
            GameObject enemyGO = GameObject.Instantiate(
                this.enemyPrefabs[Random.Range(0, this.enemyPrefabs.Length)], 
                new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), 
                Quaternion.identity);

            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemy.target = this.player;
            enemy.OnDeath += this.OnEnemyDeath;

            this.enemies.Add(enemy);
        }
	}

    void OnEnemyDeath(Enemy enemy)
    {
        this.enemies.Remove(enemy);
    }
}
