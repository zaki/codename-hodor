using UnityEngine;
using System.Collections.Generic;

public class UILoader : MonoBehaviour
{
    public List<GameObject> UIPrefabs;

    void Awake()
    {
        foreach (GameObject uiPrefab in UIPrefabs)
        {
            GameObject ui = GameObject.Instantiate(uiPrefab);
            ui.transform.SetParent(gameObject.transform, false);
        }
    }
}
