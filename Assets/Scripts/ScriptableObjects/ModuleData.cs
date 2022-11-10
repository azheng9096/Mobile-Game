using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ModuleData", order = 1)]

public class ModuleData : ScriptableObject
{
    public string modName;
    public Sprite sprite;
    public string descriptor;
    public ModuleType type;
    public int damage;
    public int accuracy;
    public string specialEffect;

    public string GetModuleType() {
        switch(type) {
            case ModuleType.Attack:
                return "Attack";
            case ModuleType.Support:
                return "Support";
        }
        return "";
    }
}

public enum ModuleType {Attack, Support};