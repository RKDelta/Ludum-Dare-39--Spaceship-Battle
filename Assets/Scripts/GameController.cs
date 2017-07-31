using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; protected set; }

    public bool isPaused;

    public Wave[] waves;
    public int waveIndex = 0;

    List<Enemy> enemies;

    public Player player;

    List<GameObject> moveRelativeToPlayer;

    public float maxXDist = 50;
    public float maxYDist = 50;

    public GameObject[] asteroidPrefabs;
    public int numAsteroids = 50;

    private void Awake()
    {
        this.enemies = new List<Enemy>();

        this.moveRelativeToPlayer = new List<GameObject>();

        if (Instance != null)
        {
            Debug.LogError("There should be no more than one GameController in the scene. There are atleast two.");
        }

        Instance = this;
    }

    void Start ()
    {
        for (int i = 0; i < this.numAsteroids; i++)
        {
            Vector2 pos = new Vector2(UnityEngine.Random.Range(3, this.maxXDist), UnityEngine.Random.Range(3, this.maxYDist));
            pos.x *= UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
            pos.y *= UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

            GameObject.Instantiate(
                this.asteroidPrefabs[UnityEngine.Random.Range(0, this.asteroidPrefabs.Length)],
                pos,
                Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
        }
	}
	
	void FixedUpdate ()
    {
        if (this.enemies.Count == 0)
        {
            foreach (EnemySpawnData data in this.waves[this.waveIndex].enemies)
            {
                for (int i = 0; i < data.count; i++)
                {
                    GameObject enemyGO;

                    if (data.randomizePosition)
                    {
                        Vector2 pos = new Vector3(UnityEngine.Random.Range(10, this.maxXDist), UnityEngine.Random.Range(10, this.maxYDist), 0);
                        pos.x *= UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                        pos.y *= UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

                        pos += (Vector2)this.player.transform.position;

                        enemyGO = GameObject.Instantiate(
                            data.enemyPrefab,
                            pos,
                            Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
                    }
                    else
                    {
                        enemyGO = GameObject.Instantiate(
                            data.enemyPrefab);
                    }

                    Enemy enemy = enemyGO.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        enemy.target = this.player;
                        enemy.OnDeath += this.OnEnemyDeath;
                    }
                    else
                    {
                        // ITS A BOSS!
                    }

                    this.enemies.Add(enemy);
                }
            }

            this.waveIndex++;
        }

        foreach (GameObject go in this.moveRelativeToPlayer)
        {
            Vector3 pos = go.transform.position;
            Vector3 relPos = pos - this.player.transform.position;

            if (relPos.x > this.maxXDist)
            {
                pos.x -= this.maxXDist * 2;
            }
            else if (relPos.x < -this.maxXDist)
            {
                pos.x += this.maxXDist * 2;
            }

            if (relPos.y > this.maxXDist)
            {
                pos.y -= this.maxXDist * 2;
            }
            else if (relPos.y < -this.maxYDist)
            {
                pos.y += this.maxXDist * 2;
            }

            go.transform.position = pos;
        }
	}

    void OnEnemyDeath(GameObject enemy)
    {
        this.enemies.Remove(enemy.GetComponent<Enemy>());
    }

    public void ObjectMoveRelativeToPlayer(GameObject gameObject, ref Action<GameObject> OnDeath)
    {
        this.moveRelativeToPlayer.Add(gameObject);

        OnDeath += (GameObject go) =>
        {
            this.moveRelativeToPlayer.Remove(go);
        };
    }
}
