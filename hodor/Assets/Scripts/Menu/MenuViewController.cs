using UnityEngine;
using System.Collections;

public class MenuViewController : ViewController<MenuViewPresenter>
{
    private ScoreModel score;

    void OnEnable()
    {
        ViewPresenter.StartGameButton.onClick.AddListener(OnStartGame);
        ViewPresenter.ExitGameButton.onClick.AddListener(() => Application.Quit());

        score = new ScoreModel();
        ViewPresenter.HiscoreText.text = string.Format("hiscore: {0}", score.Score);
    }

    void OnDisable()
    {
        ViewPresenter.StartGameButton.onClick.RemoveAllListeners();
        ViewPresenter.ExitGameButton.onClick.RemoveAllListeners();
    }

    void OnStartGame()
    {
        ViewPresenter.Hide( () => Application.LoadLevel("Field") );
    }
}
