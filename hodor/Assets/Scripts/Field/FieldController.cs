using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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

    public static UnityEvent DestroyedMole = new UnityEvent();
    public static UnityEvent GameOver = new UnityEvent();

    // Setting the root object directly as it is not really a UI element
    public GameObject TileRoot;
    public GameObject MoleRoot;
    public GameObject TilePrefab;
    public GameObject MarkerPrefab;
    public GameObject MolePrefab;

    public float SpawnInterval = 1.5f;

    private Vector2 FieldSize;
    private bool markerActive = false;
    private FieldUIController fieldUIController;

    private int score = 0;

    void Awake()
    {
        Vector2 referenceResolution = gameObject.GetComponent<CanvasScaler>().referenceResolution;
        FieldSize = FieldConstants.FieldSize(referenceResolution);
    }

    void OnEnable()
    {
        FieldController.DestroyedMole.AddListener(() => { score++; fieldUIController.Score = score; });
        FieldController.GameOver.AddListener(() => { fieldUIController.GameOver(score); });
    }

    void OnDisable()
    {
        FieldController.DestroyedMole.RemoveAllListeners();
        FieldController.GameOver.RemoveAllListeners();
    }

    void Start()
    {
        fieldUIController = GameObject.FindObjectOfType<FieldUIController>() as FieldUIController;

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
        int tries = 3;
        bool valid = false;
        Vector2 position = new Vector2();

        while (tries > 0 && !valid)
        {
            position.x = Random.Range(0, 2 * FieldSize.x * FieldConstants.TileWidth);
            position.y = Random.Range(0, 2 * FieldSize.y * FieldConstants.TileHeight);

            // Ensure two radmoles don't spawn at the same place
            RaycastHit2D hit = Physics2D.Raycast(position - 32f * Vector2.right, Vector2.left);
            if (hit && hit.collider.tag == "Proximity")
            {
                tries--;
            }
            else
            {
                valid = true;
            }
        }

        if (valid)
        {
            GameObject mole = GameObject.Instantiate(MolePrefab);
            mole.transform.SetParent(MoleRoot.transform, false);
            mole.name = "Radmole" + Random.Range(10000, 99999);

            mole.transform.position = new Vector3(position.x, position.y, 0);
            mole.transform.localScale = Vector3.one;
            float rotation = Random.Range(-180f, 180f);
            mole.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));
        }
    }
}
