using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MarkerController : SimpleViewController
{
    public float ExplosionDelay = 1.2f;
    public CircleCollider2D Collider;

    private FieldController fieldController = null;

    void OnEnable()
    {
        StartCoroutine(WaitForExplosion());
    }

    IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(ExplosionDelay);

        Collider.enabled = true;

        // Allow the collider to trigger moleicide
        yield return new WaitForSeconds(0.1f);

        fieldController.DeactivateMarker();
        Collider.enabled = false;

        GameObject.Destroy(gameObject);
    }

    public void SetController(FieldController fieldController)
    {
        this.fieldController = fieldController;
    }
}
