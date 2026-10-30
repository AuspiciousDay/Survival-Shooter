using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public UIManager uIManager;
    
    private int score = 0;
    public bool IsGameOver { get; private set;}

    private void Start()
    {
        uIManager.SetScore(score);
    }
    public void EndGame()
    {
        IsGameOver = true;
        uIManager.SetActiveGameOverUI(true);

        enemySpawner.enabled = false;
        enemySpawner.OnEndGame();
    }
    public void AddScore(int add)
    {
        if (IsGameOver)
        {
            return;
        }
        score += add;
        uIManager.SetScore(score);
    }
    public void ResartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        
    }
    public void Resume()
    {
        
    }
    public void Paused(bool active)
    {
        uIManager.SetActivePaused(active);
    }
}
