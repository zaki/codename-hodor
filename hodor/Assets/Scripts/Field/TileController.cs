using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;

public class TileController : ViewController<TileViewPresenter>
{
    private static string tileMapPath = "Art/Textures/Tiles";
    public static Sprite[] Tiles;

    public static void LoadTiles()
    {
        Tiles = Resources.LoadAll<Sprite>(tileMapPath);

        Tiles.MustNotBeNull();
        Tiles.Length.MustNotBeEqual(0);
    }

    public void Setup()
    {
        if (Random.value > 0.7f)
        {
            int tileIndex = Random.Range(0, TileController.Tiles.Length);
            ViewPresenter.TileImage.overrideSprite = TileController.Tiles[tileIndex];
        }
    }
}
