using UnityEngine;
using System.Collections;

public class MenuViewController : ViewController<MenuViewPresenter>
{
    void OnEnable()
    {
        ViewPresenter.StartGameButton.onClick.AddListener(OnStartGame);
        ViewPresenter.ExitGameButton.onClick.AddListener(() => Application.Quit());
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
