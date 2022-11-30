using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ModuleData", order = 1)]

public class ModuleData : ScriptableObject
{
    public string modName;
    public Sprite sprite;
    public string descriptor;
    public float animDelay;
    public ModuleType type;
    public int mode;
    public int damage;
    public int accuracy;
    public float cooldown;
    public string specialEffect;

    public string GetModuleType() {
        switch(type) {
            case ModuleType.Melee:
                return "Melee";
            case ModuleType.Shoot:
                return "Shoot";
            case ModuleType.Support:
                return "Support";
            case ModuleType.Drone:
                return "Drone";
        }
        return "";
    }
}

public enum ModuleType {Melee, Shoot, Support, Drone};