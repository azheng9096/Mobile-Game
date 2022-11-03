using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchDemo : MonoBehaviour
{
    Camera cam;
    public Color[] colors;
    public GameObject boxPrefab;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = cam.ScreenToWorldPoint(touch.position);
                GameObject newBox = Instantiate(boxPrefab, pos, Quaternion.identity);
                if(i<colors.Length -1){
                    newBox.GetComponent<SpriteRenderer>().color = colors[i];
                }else{
                    newBox.GetComponent<SpriteRenderer>().color = colors[colors.Length -1];
                }
            }
        }
    }
}
