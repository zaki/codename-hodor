using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using UniRx;

[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(ObjectPool))]
public class FieldController : SimpleViewController
{
    internal static class FieldConstants
    {
        public static int TileWidth  = 32;
        public static int TileHeight = 32;
        public static int FieldMargin = 1;

        public static Vector2 FieldSize(Vector2 resolution)
        {
            int fieldWidth  = Mathf.FloorToInt(resolution.x / TileWidth  / 2) + FieldMargin;
            int fieldHeight = Mathf.FloorToInt(resolution.y / TileHeight / 2) + FieldMargin;

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
    private ObjectPool radmolePool;

    private ScoreModel score;

    void Start()
    {
        Vector2 resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        FieldSize = FieldConstants.FieldSize(resolution);

        score = new ScoreModel();
        radmolePool = gameObject.GetComponent<ObjectPool>();
        fieldUIController = GameObject.FindObjectOfType<FieldUIController>() as FieldUIController;

        FieldController.DestroyedMole.AsObservable().Subscribe(_=> fieldUIController.Score.Value = ++score.Score);
        FieldController.GameOver.AsObservable().Subscribe(OnGameOver);

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
        marker.transform.position = Converter.ToWorld(position);
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
        Vector3 position = new Vector3();
        Vector2 screenPosition = new Vector2();

        while (tries > 0 && !valid)
        {
            screenPosition.x = Random.Range(0, 2 * FieldSize.x * FieldConstants.TileWidth);
            screenPosition.y = Random.Range(0, 2 * FieldSize.y * FieldConstants.TileHeight);

            position = Converter.ToWorld(screenPosition);

            // Ensure two radmoles don't spawn at the same place
            RaycastHit2D hit = Physics2D.Raycast(screenPosition - 32f * Vector2.right, Vector2.left * 32f);
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
            GameObject mole = radmolePool.Create();
            if (mole == null) return;

            mole.SetActive(true);
            mole.transform.SetParent(MoleRoot.transform, false);
            mole.name = "Radmole" + Random.Range(10000, 99999);

            mole.transform.position =  position;
            mole.transform.localScale = Vector3.one;
            float rotation = Random.Range(-180f, 180f);
            mole.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));
        }
    }

    void OnGameOver(Unit unit)
    {
        score.Save();
        fieldUIController.GameOver(score.Score);
    }
}
