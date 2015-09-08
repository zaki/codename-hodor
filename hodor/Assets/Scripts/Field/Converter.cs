using UnityEngine;
using UnityEngine.UI;

public static class Converter
{
    public static Vector3 ToWorld(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }

    public static Vector3 ToWorld(Vector2 position)
    {
        Vector3 worldPosition = new Vector3(position.x, position.y, 0.0f);
        worldPosition = ToWorld(worldPosition);
        worldPosition.z = 0f;

        return worldPosition;
    }

    public static Vector3 ToScreen(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }

    public static Vector3 ToScreen(Vector2 position)
    {
        Vector3 screenPosition = new Vector3(position.x, position.y, 0.0f);
        screenPosition = ToScreen(screenPosition);
        screenPosition.z = 0f;

        return screenPosition;
    }
}
