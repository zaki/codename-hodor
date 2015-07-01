using UnityEngine;
using System.Collections;

public class MenuViewController : ViewController<MenuViewPresenter>
{
    void Start()
    {
        // TODO Wait for UI animation to finish (set up show/hide system)
        ViewPresenter.StartGameButton.onClick.AddListener(() => Application.LoadLevel("Field"));
    }

    void Disable()
    {

    }
}
