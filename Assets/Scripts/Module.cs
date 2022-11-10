using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module {
    public Module(ModuleData moduleData) {
        this.moduleData = moduleData;
    }

    public ModuleData moduleData { get; private set; }
}
