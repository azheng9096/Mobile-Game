using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;
    public float length = 1f;

    protected SpriteRenderer spriteRenderer;

    public Transform parent;

    public bool liveMove = false;

    protected Animator animator;

    protected EntityController caller;
    // Start is called before the first frame update
    virtual public void Start()
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
    virtual public void Activate(EntityController caller, string type = "") {
        this.caller = caller;
        spriteRenderer.enabled = true;
        animator.SetTrigger("Activate");
    }
    virtual public void EndActivate() {
        spriteRenderer.enabled = false;
    }
    virtual protected void moveSelf() {
        Vector3 pos = transform.position;
        pos.x = xOffset + (length * 2) + parent.position.x;
        pos.y = yOffset + parent.position.y;
        transform.position = pos;
        spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
    }

    virtual public void dealDamage() {
        caller.dealDamage();
    }
}
