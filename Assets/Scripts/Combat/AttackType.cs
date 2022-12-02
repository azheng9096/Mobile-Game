using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackType : MonoBehaviour
{
    public string typeName;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator anim;

    public delegate void AttackCallback(string type);
    public AttackCallback callback;
    protected ModuleData mod;

    public bool isInterruptible = false;
    protected bool isInterrupted = false;
    virtual public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        spriteRenderer.enabled = false;
    }
    virtual public void Activate(ModuleData mod, AttackCallback callback = null) {
        ResetAllTriggers();
        isInterrupted = false;
        this.mod = mod;
        this.callback = callback;
        spriteRenderer.enabled = true;
        anim.SetTrigger("Activate");
    }
    virtual public void dealDamage() {
        if (callback != null && (isInterrupted || !isInterruptible)) {
            callback("dealDamage");
        }
    }

    virtual public void Interrupt() {
        isInterrupted = true;
        print("interrupted");
        spriteRenderer.enabled = false; 
    }
    
    virtual public void Blocked() {

    }

    virtual public void EndActivate() {
        spriteRenderer.enabled = false;
        if (callback != null) {
            callback("EndActivate");
        }
    }

    protected void ResetAllTriggers()
    {
        foreach (var param in anim.parameters) {
            if (param.type == AnimatorControllerParameterType.Trigger) {
                anim.ResetTrigger(param.name);
            }
        }
    }
}
