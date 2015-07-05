using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

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
    public GameObject MolePrefab;

    public float SpawnInterval = 1.5f;

    private Vector2 FieldSize;
    private bool markerActive = false;

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

        StartCoroutine(SpawnMoles());
    }

    public void OnClick(BaseEventData pointer)
    {
        if (markerActive) return;

        GameObject marker = GameObject.Instantiate(MarkerPrefab);
        marker.transform.SetParent(gameObject.transform, false);
        marker.GetComponent<MarkerController>().SetController(this);

        Vector2 position = (pointer as PointerEventData).position;
        marker.transform.position = new Vector3(position.x, position.y, 0);
        marker.transform.localScale = Vector3.one;
        markerActive = true;
    }

    public void DeactivateMarker()
    {
        // TODO Change this into a UnityEvent
        markerActive = false;
    }

    private IEnumerator SpawnMoles()
    {
        while(true)
        {
            yield return new WaitForSeconds(SpawnInterval);
            SpawnMole();
        }
    }

    private void SpawnMole()
    {
        GameObject mole = GameObject.Instantiate(MolePrefab);
        mole.transform.SetParent(gameObject.transform, false);

        Vector2 position;
        position.x = Random.Range(0, 2 * FieldSize.x * FieldConstants.TileWidth);
        position.y = Random.Range(0, 2 * FieldSize.y * FieldConstants.TileHeight);
        mole.transform.position = new Vector3(position.x, position.y, 0);
        mole.transform.localScale = Vector3.one;
        float rotation = Random.Range(-180f, 180f);
        mole.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));
    }
}
