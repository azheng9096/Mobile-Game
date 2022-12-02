using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class ViewportScaler : MonoBehaviour
{
    [SerializeField] private float _targetWidth = 20f;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (Application.isPlaying)
            ScaleViewport();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (_camera)
            ScaleViewport();
#endif
    }

    private void ScaleViewport()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float targetHeight = _targetWidth / windowAspect;
        _camera.orthographicSize = targetHeight / 2f;
    }
    
}