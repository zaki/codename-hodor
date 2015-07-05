using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasScaler))]
public class FieldController : SimpleViewController
{
    internal static class FieldConstants
    {
        public static int TileWidth  = 32;
        public static int TileHeight = 32;

        public static int FieldMargin = 1;

        public static Vector2 FieldSize(Vector2 referenceResolution)
        {
            int fieldWidth  = Mathf.FloorToInt(referenceResolution.x / TileWidth  / 2) + FieldMargin;
            int fieldHeight = Mathf.FloorToInt(referenceResolution.y / TileHeight / 2) + FieldMargin;

            return new Vector2(fieldWidth, fieldHeight);
        }
    }

    // Setting the root object directly as it is not really a UI element
    public GameObject TileRoot;
    public GameObject TilePrefab;
    public GameObject MarkerPrefab;

    private Vector2 FieldSize;
    private int fieldWidth;
    private int fieldHeight;

    void Awake()
    {
        Vector2 referenceResolution = gameObject.GetComponent<CanvasScaler>().referenceResolution;
        FieldSize = FieldConstants.FieldSize(referenceResolution);
    }

    void Start()
    {
        TileController.LoadTiles();

        for (int x = (int)-FieldSize.x; x < (int)FieldSize.x + 1; x++)
        {
            for (int y = (int)-FieldSize.y; y < (int)FieldSize.y + 1; y++)
            {
                GameObject tile = GameObject.Instantiate(TilePrefab);
                TileController tileController = tile.GetComponent<TileController>();
                tile.transform.SetParent(TileRoot.transform, false);

                tile.transform.localPosition = new Vector3(x * FieldConstants.TileWidth,
                                                           y * FieldConstants.TileHeight,
                                                           0);
                tileController.Setup();
            }
        }
    }

    public void OnClick(BaseEventData pointer)
    {
        GameObject marker = GameObject.Instantiate(MarkerPrefab);
        marker.transform.SetParent(gameObject.transform, false);

        Vector2 position = (pointer as PointerEventData).position;
        marker.transform.position = new Vector3(position.x, position.y, 0);
        marker.transform.localScale = Vector3.one;

    }
}
