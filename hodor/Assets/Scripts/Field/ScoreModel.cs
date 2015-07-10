using UnityEngine;

public class ScoreModel
{
    private readonly string ScoreKey = "HISCORE";

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;

        }
    }

    public ScoreModel()
    {
        score = PlayerPrefs.GetInt(ScoreKey, 0);
    }

    public void Save()
    {
        if (score > PlayerPrefs.GetInt(ScoreKey, 0))
        {
            PlayerPrefs.SetInt(ScoreKey, score);
        }
    }
}
