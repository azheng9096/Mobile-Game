using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;
    public float length = 1f;

    SpriteRenderer spriteRenderer;

    public Transform parent;

    public bool liveMove = false;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveSelf();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (liveMove) {
            moveSelf();
        }
        #endif
    }
    public void Shoot() {
        spriteRenderer.enabled = true;
        animator.SetTrigger("Shoot");
    }
    public void EndShoot() {
        spriteRenderer.enabled = false;
    }
    void moveSelf() {
        Vector3 pos = transform.position;
        pos.x = xOffset + (length * 2) + parent.position.x;
        pos.y = yOffset + parent.position.y;
        transform.position = pos;
        spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
    }
}
