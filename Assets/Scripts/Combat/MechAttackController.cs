using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAttackController : AttackController
{
    SpriteRenderer spriteRenderer_front;
    SpriteRenderer spriteRenderer_back;
    Animator animator_front;
    Animator animator_back;
    public override void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer_front = transform.GetChild(1).GetComponent<SpriteRenderer>();
        spriteRenderer_back = transform.GetChild(2).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator_front = transform.GetChild(1).GetComponent<Animator>();
        animator_back = transform.GetChild(2).GetComponent<Animator>();
        spriteRenderer.enabled = false;
        spriteRenderer_front.enabled = false;
        spriteRenderer_back.enabled = false;
        animator_front.SetBool("Front", true);
        animator_back.SetBool("Front", false);
        animator_back.SetTrigger("Activate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void moveSelf() {}
    public override void Activate(EntityController caller, string type = "") {
        this.caller = caller;
        if (type == "BulletRain") {
            StartCoroutine(BulletRain());
        } else {
            spriteRenderer.enabled = true;
            spriteRenderer_front.enabled = false;
            spriteRenderer_back.enabled = false;
            animator.SetTrigger("Activate");
        }
    }

    IEnumerator BulletRain() {
        //Very jank way of handling bullet rain's animation since it relies a lot on timing
        spriteRenderer.enabled = false;
        //Wait for "bullets" to come back down
        yield return new WaitForSeconds(1.5f);
        animator_front.SetTrigger("Activate");
        animator_back.SetTrigger("Activate");
        //Wait for animators to catch up
        yield return new WaitForSeconds(0.15f);
        spriteRenderer_front.enabled = true;
        spriteRenderer_back.enabled = true;
        //Wait for animators to wait until bullets land
        yield return new WaitForSeconds(0.3f);
        if (spriteRenderer_front.enabled) {
            caller.dealDamage();
        }
        //Hide the bullets
        yield return new WaitForSeconds(0.5f);
        spriteRenderer_front.enabled = false;
        spriteRenderer_back.enabled = false;
    }
    public override void EndActivate() {
        spriteRenderer.enabled = false;
        spriteRenderer_front.enabled = false;
        spriteRenderer_back.enabled = false;
    }
}
