using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MarkerController : SimpleViewController
{
    public float ExplosionDelay = 1.2f;

    void OnEnable()
    {
        StartCoroutine(WaitForExplosion());
    }

    IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(ExplosionDelay);

        // TODO Play animation
        // TODO Check hits
        // TODO Send score events

        GameObject.Destroy(gameObject);
    }
}
