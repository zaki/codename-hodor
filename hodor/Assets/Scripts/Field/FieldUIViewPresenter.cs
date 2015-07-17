using UnityEngine;
using UnityEngine.UI;

public class FieldUIViewPresenter : ViewPresenter
{
    public Text ScoreLabel;
    public Text RemainingTimeLabel;

    public GameObject GameOverRibbon;
    public Text GameOverScoreLabel;

    public void UpdateRemainingTime(float remainingTime)
    {
        RemainingTimeLabel.text = string.Format("{0:0.00}", remainingTime);

        if (remainingTime <= 5.0f)
        {
            RemainingTimeLabel.color = Color.red;
        }

        if (remainingTime <= 0.0f)
        {
            RemainingTimeLabel.text = "Time up";
        }
    }
}
