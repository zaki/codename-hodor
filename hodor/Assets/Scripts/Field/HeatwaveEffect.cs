using UnityEngine;

[ExecuteInEditMode]
public class HeatwaveEffect : MonoBehaviour
{
    public Shader HeatwaveShader;
    private Material mat;

    void Start()
    {
        mat = new Material(HeatwaveShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

}
