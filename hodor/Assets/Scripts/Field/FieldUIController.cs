using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class FieldUIController : ViewController<FieldUIViewPresenter>
{
    private float timeRemaining = 60.0f;

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
        timeRemaining -= Time.deltaTime;

        ViewPresenter.RemainingTimeLabel.text = string.Format("{0:0.00}", timeRemaining);

        if (timeRemaining <= 0.0f)
        {
            FieldController.GameOver.Invoke();
        }
    }
}
