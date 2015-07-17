using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using UniRx;

public class FieldUIController : ViewController<FieldUIViewPresenter>
{
    private float remainingTime = 60.0f;
    private bool gameRunning = true;

    public ReactiveProperty<int> Score;
    public ReactiveProperty<float> RemainingTime;

    void Start()
    {
        Score = new ReactiveProperty<int>(0);
        Score.Subscribe(UpdateScore).AddTo(this);

        RemainingTime = new ReactiveProperty<float>(60.0f);
        RemainingTime.Subscribe(UpdateRemainingTime);
    }

    void Update()
    {
        if (!gameRunning) return;

        remainingTime -= Time.deltaTime;

        ViewPresenter.UpdateRemainingTime(remainingTime);

        if (remainingTime <= 0.0f)
        {
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

    void UpdateScore(int score)
    {
        ViewPresenter.ScoreLabel.text = string.Format("{0} rms", score);
    }

    void UpdateRemainingTime(float remaining)
    {
        ViewPresenter.RemainingTimeLabel.text = string.Format("{0:0.00}", remaining);
    }
}
