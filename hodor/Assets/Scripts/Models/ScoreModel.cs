using UnityEngine;

public class ScoreModel
{
    private readonly string ScoreKey = "HISCORE";

    public int Hiscore { get; private set; }

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
        Hiscore = PlayerPrefs.GetInt(ScoreKey, 0);
    }

    public void Save()
    {
        if (score > PlayerPrefs.GetInt(ScoreKey, 0))
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            Hiscore = score;
        }
    }
}
