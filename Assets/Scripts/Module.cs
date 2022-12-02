using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module {
    public Module(ModuleData moduleData) {
        this.moduleData = moduleData;
        planningAvailable = true;
    }

    public ModuleData moduleData { get; private set; }


    // --- PLANNING ---
    public void ResetState() {
        planningAvailable = true;
    }

    public bool planningAvailable; // not used in some previous battle turn

    // can implement stacking here if wanted
}
