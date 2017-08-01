using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    public BossHealthBarController bossBar;

    public HideUI enemyIndicator;

    public MainMenu mainMenu;
    public DeathScreen deathScreen;
    public PauseScreen pauseScreen;
    public VictoryScreen victoryScreen;

    public float minEnemyIndicatorRange = 15;
    private float minEnemyIndicatorSqrRange;

    public Text waveDisplayText;

    private void Awake()
    {
        this.enemies = new List<Enemy>();

        this.moveRelativeToPlayer = new List<GameObject>();

        if (Instance != null)
        {
            Debug.LogError("There should be no more than one GameController in the scene. There are atleast two.");
        }

        Instance = this;

        this.minEnemyIndicatorSqrRange = this.minEnemyIndicatorRange * this.minEnemyIndicatorRange;

        this.deathScreen.DeactivateDeathScreen();
        this.pauseScreen.DeactivatePauseScreen();
        this.victoryScreen.DeactivateVictoryScreen();
        this.mainMenu.ActivateMainMenu();

        this.isPaused = true;
    }

    void Start()
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

        this.bossBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (this.player.Power <= 0)
        {
            this.isPaused = true;
            this.deathScreen.ActivateDeathScreen(this.waveIndex - 1);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            this.isPaused = this.isPaused == false;

            if (this.isPaused)
            {
                this.pauseScreen.ActivatePauseScreen();
            }
            else
            {
                this.pauseScreen.DeactivatePauseScreen();
            }
        }

        if (this.isPaused == false)
        {
            this.waveDisplayText.text = string.Format("WAVE: {0}", this.waveIndex);

            if (this.enemies.Count == 0 && this.waveIndex < this.waves.Length)
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

                            this.enemies.Add(enemy);
                        }
                        else
                        {
                            BossWallController wall = enemyGO.GetComponent<BossWallController>();

                            Vector2 pos = new Vector2(this.player.transform.position.x, player.transform.position.y + (wall.height / 2) - 6);

                            enemyGO.transform.position = pos;

                            // ITS A BOSS!

                            this.bossBar.boss = enemyGO.GetComponentInChildren<Boss>();

                            this.bossBar.boss.OnDestroyed += this.OnBossDestroyed;

                            this.bossBar.gameObject.SetActive(true);
                        }
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

            if (this.enemies.Count > 0)
            {
                Enemy closestEnemy = null;
                float closestSqrDist = Mathf.Infinity;

                foreach (Enemy enemy in this.enemies)
                {
                    float sqrDist = Vector3.SqrMagnitude(this.player.transform.position - enemy.transform.position);

                    if (closestEnemy == null || sqrDist < closestSqrDist)
                    {
                        closestEnemy = enemy;
                        closestSqrDist = sqrDist;
                    }
                }

                this.enemyIndicator.transform.rotation =
                    Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.up, (closestEnemy.transform.position - player.transform.position), Vector3.back));

                if (closestSqrDist > this.minEnemyIndicatorSqrRange)
                {
                    this.enemyIndicator.baseAlpha = Mathf.Clamp01(this.enemyIndicator.baseAlpha + Time.deltaTime / 2);
                }
                else
                {
                    this.enemyIndicator.baseAlpha = Mathf.Clamp01(this.enemyIndicator.baseAlpha - Time.deltaTime / 2);
                }
            }
            else
            {
                this.enemyIndicator.baseAlpha = Mathf.Clamp01(this.enemyIndicator.baseAlpha - Time.deltaTime / 2);
            }
        }
    }

    void OnBossDestroyed()
    {
        this.isPaused = true;

        this.victoryScreen.ActivateVictoryScreen();
    }

    void OnEnemyDeath(GameObject enemy)
    {
        this.enemies.Remove(enemy.GetComponent<Enemy>());
    }

    public void RemoveObjectFromRelativeMovement(GameObject gameObject)
    {
        this.moveRelativeToPlayer.Remove(gameObject);
    }

    public void ObjectMoveRelativeToPlayer(GameObject gameObject, ref Action<GameObject> OnDeath)
    {
        this.moveRelativeToPlayer.Add(gameObject);

        OnDeath += this.RemoveObjectFromRelativeMovement;
    }
}