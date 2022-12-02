using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanningUIInfoDisplay : MonoBehaviour
{
    Module module;

    public Image icon;
    public TextMeshProUGUI type;
    public TextMeshProUGUI modName;
    public TextMeshProUGUI descriptor;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI acc;
    public TextMeshProUGUI cd;
    public TextMeshProUGUI specialEffect;

    void Start() {
        ResetDisplay();
    }

    public void DisplayModule(Module module) {
        this.module = module;
        Display();
    }

    void Display() {
        icon.gameObject.SetActive(true);
        icon.sprite = module.moduleData.sprite;

        type.text = module.moduleData.GetModuleType();
        modName.text = module.moduleData.modName;
        descriptor.text = module.moduleData.descriptor;

        dmg.text = (module.moduleData.damage).ToString();
        acc.text = (module.moduleData.accuracy).ToString();
        cd.text = (module.moduleData.cooldown).ToString();
        
        specialEffect.text = module.moduleData.specialEffect;
    }

    public void ResetDisplay() {
        module = null;

        icon.gameObject.SetActive(false);

        type.text = "Type";
        modName.text = "Mod Name";
        descriptor.text = "Module descriptor";

        dmg.text = "--";
        acc.text = "--";
        cd.text = "--";

        specialEffect.text = "Module special effect description";
    }
}
