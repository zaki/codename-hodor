using UnityEngine;
using System.Collections;
using UniRx;

public class MenuViewController : ViewController<MenuViewPresenter>
{
    private ScoreModel scoreModel;

    void Start()
    {
        ViewPresenter.StartGameButton.OnClickAsObservable().Subscribe(_ => OnStartGame()).AddTo(this);
        ViewPresenter.ExitGameButton.OnClickAsObservable().Subscribe(_ => Application.Quit()).AddTo(this);

        scoreModel = new ScoreModel();
        ViewPresenter.UpdateHiscore(scoreModel.Hiscore);
    }

    void OnStartGame()
    {
        ViewPresenter.Hide( () => Application.LoadLevel("Field") );
    }
}
