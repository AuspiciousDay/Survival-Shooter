using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public UIManager uIManager;
    
    private  bool isMuted = false;
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
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Paused(false);        
    }
    public void Paused(bool active)
    {
        uIManager.SetActivePaused(active);
    }
    public void Mute()
    {
        isMuted = !isMuted;
        Mute(isMuted);
    }
    private void Mute(bool active)
    {
        isMuted = active;
        AudioListener.volume = active ? 0f : 1f;
    }
}
