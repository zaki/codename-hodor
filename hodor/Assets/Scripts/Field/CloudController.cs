using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CloudController : MonoBehaviour
{
    private ParticleSystem clouds;

    void Awake()
    {
        clouds = GetComponent<ParticleSystem>();
    }

    IEnumerator Start()
    {
        while (true)
        {
            clouds.Play(true);
            yield return new WaitForSeconds(10.0f);
            clouds.Stop();
            yield return new WaitForSeconds(Random.Range(5.0f, 15.0f));
        }
    }

}
