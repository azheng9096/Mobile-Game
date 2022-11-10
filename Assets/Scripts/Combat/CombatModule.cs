using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatModule : MonoBehaviour
{
    Module module;
    CombatManager combatManager;
    public void Init(Module module, CombatManager combatManager) {
        this.module = module;
        this.combatManager = combatManager;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UseModule() {
        print("Module used");
        Destroy(gameObject);
    }
}
