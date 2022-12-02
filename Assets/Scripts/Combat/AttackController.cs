using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackController : MonoBehaviour
{
    public List<AttackType> attackTypes;

    Dictionary<string, AttackType> attackTypeDict;

    protected EntityController caller;

    protected ModuleData mod;

    virtual public void Start()
    {
        attackTypeDict = new Dictionary<string, AttackType>();
        foreach (AttackType attackType in attackTypes) {
            attackTypeDict.Add(attackType.typeName, attackType);
        }
    }
    virtual public void Activate(EntityController caller, string type, ModuleData mod = null) {
        this.mod = mod;
        this.caller = caller;
        if (attackTypeDict.ContainsKey(type)) {
            attackTypeDict[type].Activate(mod, AttackCallback);
        }
    }
    
    virtual public void Blocked() {
        foreach (AttackType attackType in attackTypes) {
            if (attackType.spriteRenderer.enabled) {
                attackType.Blocked();
            }
        }
    }
    virtual public void Interrupt() {
        foreach (AttackType attackType in attackTypes) {
            if (attackType.spriteRenderer.enabled) {
                attackType.Interrupt();
            }
        }
    }
    virtual public void AttackCallback(string type) {
        if (type == "dealDamage") {
            dealDamage();
        } else if (type == "HaltBlock") {
            caller.HaltBlock();
        }
    }
    virtual public void dealDamage() {
        caller.dealDamage();
    }
}
