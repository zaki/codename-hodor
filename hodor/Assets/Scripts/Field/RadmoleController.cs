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
    private float speed = 50.0f;
    private int ignoreRaycastLayer;

    void Awake()
    {
        MoleAnimator = gameObject.GetComponent<Animator>();
        ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void OnEnable()
    {
        StartCoroutine(MoveToBurrow());
    }

    void Update()
    {
        if (moving)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * speed);
        }

        // TODO No magic numbers
        if (gameObject.transform.position.x < -32 || gameObject.transform.position.x > 800 ||
            gameObject.transform.position.y < -32 || gameObject.transform.position.y > 600)
        {
            // Wandered off-screen
            StopAllCoroutines();
            GameObject.Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            // TODO Improve avoidance detection
            int layer = gameObject.layer;

            Vector2 forward = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.y);
            Vector3 start = gameObject.transform.position + gameObject.transform.forward * 35f;

            // Disable self-collision in raycast
            Proximity.enabled = false;
            ProximityBody.Sleep();
            gameObject.layer = ignoreRaycastLayer;

            // Look ahead, turn if necessary
            RaycastHit2D hit = Physics2D.Raycast(start, forward, 64.0f, ProximityLayerMask);
            Debug.DrawRay(start, gameObject.transform.forward, Color.blue, 0.1f);

            if (hit)
            {
                float turn = Random.Range(-15f, 15f);
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
        string baseLayer = MoleAnimator.GetLayerName(0);
        string moveAnim = string.Format("{0}.Move", baseLayer);

        while (MoleAnimator.IsInTransition(0) || !MoleAnimator.GetCurrentAnimatorStateInfo(0).IsName(moveAnim))
        {
            yield return null;
        }

        moving = true;
        Collider.enabled = true;

        yield return new WaitForSeconds(moveDuration);

        MoleAnimator.SetTrigger("DoBurrow");
        Collider.enabled = false;

        yield return new WaitForEndOfFrame();

        // Wait for burrow to finish
        while (MoleAnimator.IsInTransition(0) || MoleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "TargetMarker")
        {
            StopAllCoroutines();

            Collider.enabled = false;
            moving = false;

            StartCoroutine(DoDie());
        }
    }

    IEnumerator DoDie()
    {
        MoleAnimator.SetTrigger("DoDie");
        FieldController.DestroyedMole.Invoke();

        yield return new WaitForEndOfFrame();

        while (MoleAnimator.IsInTransition(0) || MoleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Let the remains lie around for a while... for... artistic purposes of course
        yield return new WaitForSeconds(1.5f);

        GameObject.Destroy(gameObject);
    }
}
