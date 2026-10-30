using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uIManager;
    public Enemy[] enemyPrefabs = new Enemy[3];
    public Transform[] spawnPoints;

    public float spawnTime = 5f;

    private readonly List<Enemy> enemies = new List<Enemy>();
    private readonly List<Enemy> enemyPool = new List<Enemy>();

    private int wave = 0;

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameController");
        gameManager = findGo.GetComponent<GameManager>();
        uIManager = gameManager.uIManager;
    }

    private void Update()
    {
        if(enemies.Count == 0)
        {
            SpawnWave();
        }   
    }
    private void SpawnWave()
    {
        wave++;

        int count = Mathf.RoundToInt(wave * 3.5f);
        for(int i = 0; i < count; i++)
        {
            CreateEnemy();
        }
        uIManager.SetWaveText(wave, enemies.Count);
    }

    private void CreateEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy;
        if(enemyPool.Count == 0)
        {
            enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);

            enemy.OnDead.AddListener(() => enemies.Remove(enemy));
            enemy.OnDead.AddListener(() => uIManager.SetWaveText(wave, enemies.Count));
            enemy.OnDead.AddListener(() => StartCoroutine(CoReturnToPoolEnemies(enemy)));
            enemy.OnDead.AddListener(() => gameManager.AddScore(enemy.score));            
        }
        enemy = enemyPool[0];
        enemy.transform.position = point.position;
        enemy.transform.rotation = point.rotation;
        enemy.gameObject.SetActive(true);

        enemies.Add(enemy);
        enemyPool.Remove(enemy);
    }

    private IEnumerator CoReturnToPoolEnemies(Enemy enemy)
    {
        yield return new WaitForSeconds(spawnTime);
        enemy.gameObject.SetActive(false);
        enemyPool.Remove(enemy);
    }

    public void OnEndGame()
    {
        for(int i = enemies.Count - 1; i >=0 ; i--)
        {
            enemies[i].Die();
        }
    }
}
