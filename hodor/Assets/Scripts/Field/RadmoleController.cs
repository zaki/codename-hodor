using UnityEngine;
using System.Collections;

public class RadmoleController : SimpleViewController
{
    public BoxCollider2D Collider;
    public CircleCollider2D Proximity;
    public Rigidbody2D ProximityBody;
    public LayerMask ProximityLayerMask;

    private Animator MoleAnimator;
    private float moveDuration = 5.0f;
    private bool moving = false;
    private float speed = 0.05f;
    private int ignoreRaycastLayer;
    private float turn = 0.0f;

    private int screenHeight;
    private int screenWidth;

    void Awake()
    {
        MoleAnimator = gameObject.GetComponent<Animator>();
        ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void OnEnable()
    {
        moveDuration = Random.Range(3.0f, 8.0f);
        speed = Random.Range(0.5f, 1.0f);
        moving = false;
    }

    void Update()
    {
        if (moving)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * speed);
        }

        if (gameObject.transform.position.x < -32 || gameObject.transform.position.x > Camera.main.pixelWidth ||
            gameObject.transform.position.y < -32 || gameObject.transform.position.y > Camera.main.pixelHeight)
        {
            // Wandered off-screen
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            int layer = gameObject.layer;

            Vector2 forward = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.y);
            Vector3 start = gameObject.transform.position + 35f * gameObject.transform.forward;

            // Disable self-collision in raycast
            Proximity.enabled = false;
            ProximityBody.Sleep();
            gameObject.layer = ignoreRaycastLayer;

            // Look ahead, turn if necessary
            RaycastHit2D hit = Physics2D.Raycast(start, forward, 256f, ProximityLayerMask);

            if (hit)
            {
                Vector3 obstaclePosition = hit.collider.gameObject.transform.position;
                Vector3 cross = Vector3.Cross(obstaclePosition, gameObject.transform.position).normalized;

                // Check the relation of our heading to their position and turn to avoid
                turn = cross.z > 0.0f ? 10.0f : -10.0f;

                gameObject.transform.Rotate(Vector3.forward, turn);
            }

            // Re-enable collision
            Proximity.enabled = true;
            ProximityBody.WakeUp();
            gameObject.layer = layer;
        }
    }

    IEnumerator MoveToBurrow()
    {
        moving = true;
        Collider.enabled = true;

        yield return new WaitForSeconds(moveDuration);

        MoleAnimator.SetTrigger("DoBurrow");
        Collider.enabled = false;
    }

    void Burrowed()
    {
        moving = false;
        gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "TargetMarker")
        {
            StopAllCoroutines();

            Collider.enabled = false;
            moving = false;

            MoleAnimator.SetTrigger("DoDie");
            FieldController.DestroyedMole.Invoke();
        }
    }

    IEnumerator Died()
    {
        moving = false;
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
