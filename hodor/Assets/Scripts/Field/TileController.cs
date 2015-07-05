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
        // TODO For now, just go with a random tile. Eventually specify the tiles used
        float random = Random.value;
        if (random > 0.9f)
        {
            int tileIndex = Random.Range(0, TileController.Tiles.Length);
            ViewPresenter.TileImage.overrideSprite = TileController.Tiles[tileIndex];
        }

        // Flip randomly
        float x = Random.value > 0.5f ? -1 : 1;
        float y = Random.value > 0.5f ? -1 : 1;
        ViewPresenter.TileImage.transform.localScale = new Vector3(x, y, 1);
    }
}
