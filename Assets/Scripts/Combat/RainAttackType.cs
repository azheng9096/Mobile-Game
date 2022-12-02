using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAttackType : AttackType
{
    public float upTime = 2f;
    public SpriteRenderer rainBack;
    public Animator backAnim;
    public override void Start() {
        base.Start();
        rainBack.enabled = false;
        anim.SetBool("Front", true);
        backAnim.SetBool("Front", false);
    }
    public override void Activate(ModuleData mod, AttackCallback callback = null)
    {
        this.ResetAllTriggers();
        this.isInterrupted = false;
        this.mod = mod;
        this.callback = callback;
        print("there");
        StartCoroutine(DelayAttack());
    }
    
    IEnumerator DelayAttack() {
        yield return new WaitForSeconds(upTime);
        print("hi");
        anim.SetTrigger("Activate");
        backAnim.SetTrigger("Activate");
        spriteRenderer.enabled = true;
        rainBack.enabled = true;
    }
    public override void Interrupt() {
        //Once bullets are shot they cannot be interrupted
        return;
    }
    public override void EndActivate()
    {
        spriteRenderer.enabled = false;
        rainBack.enabled = false;
        if (callback != null) {
            callback("EndActivate");
        }
    }
}
