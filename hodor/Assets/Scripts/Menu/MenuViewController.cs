using UnityEngine;
using System.Collections;

public class MenuViewController : ViewController<MenuViewPresenter>
{
    void Start()
    {
        ViewPresenter.StartGameButton.onClick.AddListener(OnStartGame);
    }

    void OnStartGame()
    {
        ViewPresenter.Hide( () => Application.LoadLevel("Field") );
    }
}
