using UnityEngine;
using System.Collections;

public class RadmoleController : SimpleViewController
{
    public BoxCollider2D Collider;

    private Animator MoleAnimator;
    private float moveDuration = 2.0f;
    private bool moving = false;

    void Awake()
    {
        MoleAnimator = gameObject.GetComponent<Animator>();
    }

    void OnEnable()
    {
        StartCoroutine(MoveToBurrow());
    }

    void Update()
    {
        if (moving)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * 50);
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

        // TODO Burrowed mole should probably affect score
        GameObject.Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "TargetMarker")
        {
            StopAllCoroutines();

            Collider.enabled = false;
            moving = false;
            // TODO Send score event
            MoleAnimator.SetTrigger("DoDie");

            StartCoroutine(DoDie());
        }
    }

    IEnumerator DoDie()
    {
        while (MoleAnimator.IsInTransition(0) || MoleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Let the remains lie around for a while... for... artistic purposes of course
        yield return new WaitForSeconds(1.5f);

        GameObject.Destroy(gameObject);
    }
}
