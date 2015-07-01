using UnityEngine;

public class Startup : MonoBehaviour
{
    private GameObject game = null;
    private const string gameObjectName = "GameRoot_Hodor";

    public string StartScene = "Title";

    public void Awake()
    {
        game = GameObject.Find(gameObjectName);

        if (game == null)
        {
            if (Application.loadedLevelName == StartScene)
            {
                game = new GameObject(gameObjectName);
                GameObject.DontDestroyOnLoad(game);
            }
            else
            {
                Application.LoadLevel(StartScene);
            }
        }
    }
}

