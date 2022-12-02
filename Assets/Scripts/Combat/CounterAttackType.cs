using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttackType : AttackType
{
    public float upTime = 1f;

    public AttackType laser;
    public override void Activate(ModuleData mod, AttackCallback callback = null)
    {
        anim.SetBool("Interrupt", false);
        base.Activate(mod, callback);
        StartCoroutine(DelayDissolve());
    }
    public override void Blocked() {
        anim.SetTrigger("Shoot");
        if (callback != null) {
            callback("HaltBlock");
        }
    }
    IEnumerator DelayDissolve() {
        yield return new WaitForSeconds(upTime);
        if (callback != null) {
            callback("HaltBlock");
        }
        anim.SetTrigger("Dissolve");
    }

    public override void Interrupt() {
        anim.SetBool("Interrupt", true);
        spriteRenderer.enabled = false;
        laser.Interrupt();
    }
    public void ShootLaser() {
        laser.Activate(mod);
    }
    public override void EndActivate()
    {
        spriteRenderer.enabled = false;
        if (callback != null) {
            callback("EndActivate");
        }
    }
}
