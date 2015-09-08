using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UICamera : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
