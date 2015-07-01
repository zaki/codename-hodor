using UnityEngine;
using System.Collections;

public class TitleController : SimpleViewController
{
    private const float splashTimeout = 1.0f;

    void Start()
    {
        StartCoroutine(ShowSplash());
    }

    IEnumerator ShowSplash()
    {
        // TODO Show an actual title splash. For now, just set up a quick transition to the menu
        yield return new WaitForSeconds(splashTimeout);

        Application.LoadLevel("Menu");
    }
}
