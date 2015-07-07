using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class FieldUIController : ViewController<FieldUIViewPresenter>
{
    private float timeRemaining = 60.0f;
    private bool gameRunning = true;

    public int Score
    {
        set { ViewPresenter.ScoreLabel.text = string.Format("{0} rms", value); }
    }

    public void StartTimer(float remainingTime)
    {
        timeRemaining = remainingTime;
    }

    void Update()
    {
        if (!gameRunning) return;

        timeRemaining -= Time.deltaTime;

        ViewPresenter.RemainingTimeLabel.text = string.Format("{0:0.00}", timeRemaining);

        if (timeRemaining <= 5.0f)
        {
            ViewPresenter.RemainingTimeLabel.color = Color.red;
        }

        if (timeRemaining <= 0.0f)
        {
            ViewPresenter.RemainingTimeLabel.text = "Time up";
            FieldController.GameOver.Invoke();
            gameRunning = false;
        }
    }

    public void GameOver(int score)
    {
        ViewPresenter.GameOverScoreLabel.text = string.Format("you have eradicated {0} moles this time", score);
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        ViewPresenter.GameOverRibbon.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        Application.LoadLevel("Menu");
    }
}
