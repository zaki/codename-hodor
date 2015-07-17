using UnityEngine;
using UnityEngine.UI;

public class MenuViewPresenter : ViewPresenter
{
    public Button StartGameButton;
    public Button ExitGameButton;

    public Text HiscoreText;

    public void UpdateHiscore(int hiscore)
    {
        HiscoreText.text = string.Format("hiscore: {0}", hiscore);
    }
}
