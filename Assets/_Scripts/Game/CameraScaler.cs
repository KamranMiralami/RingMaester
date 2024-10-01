using RingMaester;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetAspect = 9f / 16f;

    void Start()
    {
        ScaleCamera();
    }

    void ScaleCamera()
    {
        if (targetAspect <= 0) return;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = screenAspect / targetAspect;
        GameDebug.Log("Scaling Camera with " + scaleHeight+" "+ screenAspect+ " "+targetAspect,GameDebug.DebugEnum.StartUp);
        var camera = GetComponent<Camera>();
        var size = camera.orthographicSize / scaleHeight;
        size = Mathf.Clamp(size, 2.5f, 15f);
        camera.orthographicSize = size;
    }
}
