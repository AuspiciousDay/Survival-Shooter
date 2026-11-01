using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text waveInfoText;
    public Text scoreText;

    public GameObject gameOverUI;
    public GameObject pausedUI;

    private void OnEnable()
    {
        SetActiveGameOverUI(false);
        SetActivePaused(false);
    }
    public void SetWaveText(int wave, int count)
    {
        waveInfoText.text = $"Wave: {wave}\nEnemy Left: {count}";
    }
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
    public void SetActivePaused(bool active)
    {
        Time.timeScale = active ? 0f : 1f;
        pausedUI.SetActive(active);
    }
}
